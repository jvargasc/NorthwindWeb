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
	public class ServiceCustomers
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();

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

		public async Task<List<Customers>> GetCustomers()
		{
			List<Customers> customers = new List<Customers>();

			var response = await _httpClient.GetAsync("api/customers/getcustomers");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				customers = JsonConvert.DeserializeObject<List<Customers>>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(List<Customers>));
				customers = (List<Customers>)serializer.Deserialize(new StringReader(content.Result));
			}

			return customers;
		}

		public async Task<Customers> GetCustomer(string customerId)
		{
			Customers customer = new Customers();

			var response = await _httpClient.GetAsync($"api/customers/getcustomer/{customerId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				customer = JsonConvert.DeserializeObject<Customers>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(List<Customers>));
				customer = (Customers)serializer.Deserialize(new StringReader(content.Result));
			}

			return customer;
		}
	}
}
