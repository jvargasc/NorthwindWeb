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
	public class ServiceSuppliers
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();

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

		public async Task<List<Suppliers>> GetSuppliers()
		{
			List<Suppliers> suppliers = new List<Suppliers>();

			var response = await _httpClient.GetAsync("api/suppliers/getsuppliers");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				suppliers = JsonConvert.DeserializeObject<List<Suppliers>>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(List<Suppliers>));
				suppliers = (List<Suppliers>)serializer.Deserialize(new StringReader(content.Result));
			}

			return suppliers;
		}

	}
}
