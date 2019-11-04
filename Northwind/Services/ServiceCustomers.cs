using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Northwind.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Northwind.Services
{
	public class ServiceCustomers
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();
        private const string apiRoute = "api/customers";
        private const string mediaType = "application/json";

        public ServiceCustomers(IConfiguration configuration)
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

		public async Task<List<Customers>> GetCustomers(int page = 0, int itemsPerPage = 0)
		{
			List<Customers> customers = new List<Customers>();

			var response = await _httpClient.GetAsync($"{apiRoute}/getcustomers?page={page.ToString()}&itemsPerPage={itemsPerPage.ToString()}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				customers = JsonConvert.DeserializeObject<List<Customers>>(content.Result);
			}

			return customers;
		}

		public async Task<Customers> GetCustomer(string customerId)
		{
			Customers customer = new Customers();

			var response = await _httpClient.GetAsync($"{apiRoute}/getcustomer/{customerId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				customer = JsonConvert.DeserializeObject<Customers>(content.Result);
			}

			return customer;
		}

        public async Task<CustomersForCreation> CreateCustomer(CustomersForCreation customersToCreate)
        {
            var serializedCustomerToCreate = JsonConvert.SerializeObject(customersToCreate);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiRoute}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            request.Content = new StringContent(serializedCustomerToCreate);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdCustomer = JsonConvert.DeserializeObject<CustomersForCreation>(content);

            return createdCustomer;
        }

        public async Task UpdateCustomer(CustomersForUpdate customerToUpdate)
        {
            var serializedCustomerToUpdate = JsonConvert.SerializeObject(customerToUpdate);

            var request = new HttpRequestMessage(HttpMethod.Put,
                $"{apiRoute}/{customerToUpdate.CustomerId}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Content = new StringContent(serializedCustomerToUpdate);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }

        public async Task DeleteCustomer(string customerId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
                $"{apiRoute}/{customerId}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> CustomerExists(string customerId)
        {
            bool customerExists = false;

            var response = await _httpClient.GetAsync($"{apiRoute}/getcustomer/{customerId}");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync();

            if (response.Content.Headers.ContentType.MediaType == mediaType)
            {
                customerExists = JsonConvert.DeserializeObject<bool>(content.Result);
            }

            return customerExists;
        }
    }
}
