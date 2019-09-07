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
	public class ServiceTerritories
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();

		public ServiceTerritories(IConfiguration configuration)
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

		public async Task<List<Territories>> GetTerritories()
		{
			List<Territories> territories = new List<Territories>();

			var response = await _httpClient.GetAsync("api/territories/getterritories");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				territories = JsonConvert.DeserializeObject<List<Territories>>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(List<Territories>));
				territories = (List<Territories>)serializer.Deserialize(new StringReader(content.Result));
			}

			return territories;
		}

		public async Task<Territories> GetTerritory(string territoryId)
		{
			Territories territories = new Territories();

			var response = await _httpClient.GetAsync($"api/territories/getterritory/{territoryId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				territories = JsonConvert.DeserializeObject<Territories>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(Territories));
				territories = (Territories)serializer.Deserialize(new StringReader(content.Result));
			}

			return territories;
		}
	}
}
