using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Northwind.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Northwind.Services
{
	public class ServiceSuppliers
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();
        private const string apiRoute = "api/suppliers";
        private const string mediaType = "application/json";

        public ServiceSuppliers(IConfiguration configuration)
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

		public async Task<int> GetCount()
		{
			int count = 0;

			var response = await _httpClient.GetAsync($"{apiRoute}/getcount");
			response.EnsureSuccessStatusCode();

			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
				count = JsonConvert.DeserializeObject<int>(content.Result);

			return count;
		}

		public async Task<List<Suppliers>> GetSuppliers(int page = 0, int itemsPerPage = 0)
		{
			List<Suppliers> suppliers = new List<Suppliers>();

			var response = await _httpClient.GetAsync($"{apiRoute}/getsuppliers?page={page.ToString()}&itemsPerPage={itemsPerPage.ToString()}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				suppliers = JsonConvert.DeserializeObject<List<Suppliers>>(content.Result);
			}

			return suppliers;
		}

		public async Task<Suppliers> GetSupplier(int supplierId)
		{
			Suppliers supplier = new Suppliers();

			var response = await _httpClient.GetAsync($"{apiRoute}/getsupplier/{supplierId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				supplier = JsonConvert.DeserializeObject<Suppliers>(content.Result);
			}

			return supplier;
		}

        public async Task<SuppliersForCreation> CreateSupplier(SuppliersForCreation supplierToCreate)
        {
            var serializedSupplierToCreate = JsonConvert.SerializeObject(supplierToCreate);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiRoute}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            request.Content = new StringContent(serializedSupplierToCreate);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdSupplier = JsonConvert.DeserializeObject<SuppliersForCreation>(content);

            return createdSupplier;

        }

        public async Task UpdateSupplier(SuppliersForUpdate supplierToUpdate)
        {
            var serializedSupplierToUpdate = JsonConvert.SerializeObject(supplierToUpdate);

            var request = new HttpRequestMessage(HttpMethod.Put,
                $"{apiRoute}/{supplierToUpdate.SupplierId}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Content = new StringContent(serializedSupplierToUpdate);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }

		public async Task DeleteSupplier(int supplierId)
		{
			var request = new HttpRequestMessage(HttpMethod.Delete,
				$"{apiRoute}/{supplierId}");

			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();

		}

		public async Task <bool> SupplierExists(int supplierId)
		{
			bool supplierExists = false;

			var response = await _httpClient.GetAsync($"{apiRoute}/getsupplier");
			response.EnsureSuccessStatusCode();

			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
				supplierExists = JsonConvert.DeserializeObject<bool>(content.Result);

			return supplierExists;
		}

	}
}
