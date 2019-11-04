using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Northwind.Models;
using Northwind.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind
{
	public static class Utilities
	{
		public class PictureFile
		{
			public string Id { get; set; }
			public byte[] TmpPicture { get; set; }
		}

		public enum DetailService
		{
			Category,
			Customer,
			Employee,
			Region
		}

		public static string Base64String(byte[] Picture)
		{
			var base64Str = string.Empty;
			using (var ms = new MemoryStream())
			{
				int offset = 78;
				ms.Write(Picture, offset, Picture.Length - offset);
				var bmp = new System.Drawing.Bitmap(ms);
				using (var jpegms = new MemoryStream())
				{
					bmp.Save(jpegms, System.Drawing.Imaging.ImageFormat.Jpeg);
					base64Str = Convert.ToBase64String(jpegms.ToArray());
				}
			}
			return base64Str;
		}

		public static async Task<byte[]> ConvertPictureToBytes(IFormFile fileToConvert)
		{
			var filePath = Path.GetTempFileName();

			if (fileToConvert.Length > 0)
			{
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await fileToConvert.CopyToAsync(stream);
				}
			}

			byte[] offset = new byte[78];

			byte[] fs = Combine(offset, File.ReadAllBytes(filePath));
			return fs;
		}

		private static byte[] Combine(byte[] first, byte[] second)
		{

			byte[] ret = new byte[first.Length + second.Length];

			Buffer.BlockCopy(first, 0, ret, 0, first.Length);
			Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

			return ret;
		}

		public async static Task<List<Categories>> GetCategories(IConfiguration _configuration)
		{
			ServiceCategories serviceCategory = new ServiceCategories(_configuration);

			List<Categories> categoryList = await serviceCategory.GetCategories();

			categoryList.Insert(0, new Categories { CategoryName = "" });
			return categoryList;
		}

		public async static Task<List<Regions>> GetRegions(IConfiguration _configuration)
		{
			ServiceRegion serviceRegion = new ServiceRegion(_configuration);

			List<Regions> regionList = await serviceRegion.GetRegions();
			
			regionList.Insert(0, new Regions { RegionDescription = ""});
			return regionList;
		}

		public async static Task<List<Shippers>> GetShippers(IConfiguration _configuration)
		{
			ServiceShippers serviceShippers = new ServiceShippers(_configuration);

			List<Shippers> shippersList = await serviceShippers.GetShippers();

			shippersList.Insert(0, new Shippers { CompanyName = "" });
			return shippersList;
		}

		public async static Task<List<Suppliers>> GetSuppliers(IConfiguration _configuration)
		{
			ServiceSuppliers serviceSuppliers = new ServiceSuppliers(_configuration);

			List<Suppliers> suppliersList = await serviceSuppliers.GetSuppliers();

			suppliersList.Insert(0, new Suppliers { CompanyName = "" });
			return suppliersList;
		}

		public static string GetUrl(IConfiguration configuration)
		{
			string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			string url;

			if (env == "Development")
				url = configuration["UrlList:UrlDevelopment"];
			else
				url = configuration["UrlList:UrlProduction"];

			return url;
		}

		public async static Task<string> GetDetail(string code, DetailService detailService, IConfiguration _configuration)
		{
			string returnValue = string.Empty;

			switch(detailService)
			{
				case DetailService.Category:
					ServiceCategories serviceCategories = new ServiceCategories(_configuration);

					var Category = await serviceCategories.GetCategory(int.Parse(code));
					returnValue = Category.CategoryName;

					break;
				case DetailService.Customer:
					ServiceCustomers serviceCustomers = new ServiceCustomers(_configuration);

					var Customer = await serviceCustomers.GetCustomer(code);
					returnValue = Customer.ContactName;

					break;
				case DetailService.Employee:
					ServiceEmployees serviceEmployees = new ServiceEmployees(_configuration);

					var Employee = await serviceEmployees.GetEmployee(int.Parse(code));
					returnValue = Employee.FirstName + ' ' + Employee.LastName;

					break;
				case DetailService.Region:
					ServiceRegion serviceRegion = new ServiceRegion(_configuration);

					var Region = await serviceRegion.GetRegion(int.Parse(code));
					returnValue = Region.RegionDescription;

					break;
			}
			return returnValue;
		}

		public async static Task<IEnumerable<SelectListItem>> FillCategoriesCollection(IConfiguration _configuration)
		{
			List<Categories> categoriesList = await Utilities.GetCategories(_configuration);

			return categoriesList.Select(n => new SelectListItem
			{
				Value = n.CategoryId.ToString(),
				Text = n.CategoryName.Trim()
			}).ToList();
		}

		public async static Task<IEnumerable<SelectListItem>> FillRegionsCollection(IConfiguration _configuration)
		{
			List<Regions> regionsList = await Utilities.GetRegions(_configuration);

			return regionsList.Select(n => new SelectListItem
			{
				Value = n.RegionId.ToString(),
				Text = n.RegionDescription.Trim()
			}).ToList();
		}

		public async static Task<IEnumerable<SelectListItem>> FillShipViaCollection(IConfiguration _configuration)
		{
			List<Shippers> shipViaList = await Utilities.GetShippers(_configuration);

			return shipViaList.Select(n => new SelectListItem
			{
				Value = n.ShipperId.ToString(),
				Text = n.CompanyName.Trim()
			}).ToList();
		}

		public async static Task<IEnumerable<SelectListItem>> FillSuppliersCollection(IConfiguration _configuration)
		{
			List<Suppliers> shipViaList = await Utilities.GetSuppliers(_configuration);
			
			return shipViaList.Select(n => new SelectListItem
			{
				Value = n.SupplierId.ToString(),
				Text = n.CompanyName.Trim()
			}).ToList();
		}

		public static List<OrderDetails> GetOrderDetails(int orderId, IFormCollection formCollection)
		{

			int rowCounterInForm = int.Parse(formCollection.Where(c => c.Key == "rowCounter").FirstOrDefault().Value.ToString());
			var tmpCollection = formCollection.Where(c => c.Key.ToString().Contains("input")).ToList();
			int rowCounter = tmpCollection.Count / 6; // 6 is the number of columns per row
			int itemCounter = tmpCollection.Count / rowCounterInForm;
			int varControl = 0;
			int switchControl = 0;

			Console.Clear();
			List<OrderDetails> tmpOrderDetailsList = new List<OrderDetails>();

			for (int i = 0; i < (rowCounter); i++)
			{
				OrderDetails tmpOrderDetails = new OrderDetails();
				for (int j = varControl; j < (varControl + itemCounter); j++)
				{
					Console.WriteLine($"Key: {tmpCollection[i + j].Key} - Value: {tmpCollection[i + j].Value}");
					switch (j - varControl)
					{
						case 0:
							tmpOrderDetails.Quantity = short.Parse(tmpCollection[i + j].Value);
							break;
						case 1:
							tmpOrderDetails.ProductId = int.Parse(tmpCollection[i + j].Value);
							break;
						case 4:
							tmpOrderDetails.UnitPrice = decimal.Parse(tmpCollection[i + j].Value);
							break;
						case 5:
							tmpOrderDetails.Discount = float.Parse(tmpCollection[i + j].Value);
							break;
					}
					switchControl = j;
				}

				Console.WriteLine();
				if (orderId > 0)
					tmpOrderDetails.OrderId = orderId;

				tmpOrderDetailsList.Add(tmpOrderDetails);
				varControl = switchControl;
			}

			return tmpOrderDetailsList;
		}

	}
}
