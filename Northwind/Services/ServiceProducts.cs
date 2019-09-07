using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Northwind.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Northwind.Services
{
	public class ServiceProducts
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();

		public ServiceProducts(IConfiguration configuration)
		{
			_configuration = configuration;

			string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			string url;

			if (env == "Development")
				url = _configuration["UrlList:UrlDevelopment"];
			else
				url = _configuration["UrlList:UrlProduction"];

			_httpClient.BaseAddress = new Uri(url);
			_httpClient.Timeout = new TimeSpan(0, 0, 30);
			_httpClient.DefaultRequestHeaders.Clear();
		}

		public async Task<List<Products>> GetProducts()
		{
			List<Products> products = new List<Products>();

			var response = await _httpClient.GetAsync("api/products/getproducts");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				products = JsonConvert.DeserializeObject<List<Products>>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(List<Products>));
				products = (List<Products>)serializer.Deserialize(new StringReader(content.Result));
			}

			return products;
		}
		
		public async Task<Products> GetProduct(int productId)
		{
			Products product = new Products();

			var response = await _httpClient.GetAsync($"api/products/getproduct/{productId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				product = JsonConvert.DeserializeObject<Products>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(Products));
				product = (Products)serializer.Deserialize(new StringReader(content.Result));
			}

			return product;
		}
	}
}
