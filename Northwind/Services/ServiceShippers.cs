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
	public class ServiceShippers
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();

		public ServiceShippers(IConfiguration configuration)
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

		public async Task<List<Shippers>> GetShippers()
		{
			List<Shippers> shippers = new List<Shippers>();

			var response = await _httpClient.GetAsync("api/shippers/getshippers");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				shippers = JsonConvert.DeserializeObject<List<Shippers>>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(List<Shippers>));
				shippers = (List<Shippers>)serializer.Deserialize(new StringReader(content.Result));
			}

			return shippers;
		}

		public async Task<Shippers> GetShipper(int shipperId)
		{
			Shippers shipper = new Shippers();

			var response = await _httpClient.GetAsync($"api/shippers/getshipper/{shipperId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				shipper = JsonConvert.DeserializeObject<Shippers>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(Shippers));
				shipper = (Shippers)serializer.Deserialize(new StringReader(content.Result));
			}

			return shipper;
		}
	}
}
