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
	public class ServiceTerritories
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();
		private const string apiRoute = "api/territories";
		private const string mediaType = "application/json";

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

		public async Task<List<Territories>> GetTerritories(int page = 0, int itemsPerPage = 0)
		{
			List<Territories> territories = new List<Territories>();

			var response = await _httpClient.GetAsync($"{apiRoute}/getterritories?page={page.ToString()}&itemsPerPage={itemsPerPage.ToString()}");
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

		public async Task<TerritoriesForCreation> CreateTerritory(TerritoriesForCreation territoryToCreation)
		{
			var serializedTerritoryToCreation = JsonConvert.SerializeObject(territoryToCreation);

			var request = new HttpRequestMessage(HttpMethod.Post, $"{apiRoute}");
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

			request.Content = new StringContent(serializedTerritoryToCreation);
			request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var createdSupplier = JsonConvert.DeserializeObject<TerritoriesForCreation>(content);

			return createdSupplier;

		}

		public async Task UpdateTerritory(TerritoriesForUpdate territoryToUpdate)
		{
			var serializedTerritoryToUpdate = JsonConvert.SerializeObject(territoryToUpdate);

			var request = new HttpRequestMessage(HttpMethod.Put,
				$"{apiRoute}/{territoryToUpdate.TerritoryId}");

			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
			request.Content = new StringContent(serializedTerritoryToUpdate);
			request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();

		}

		public async Task DeleteTerritory(string territoryId)
		{
			var request = new HttpRequestMessage(HttpMethod.Delete,
				$"{apiRoute}/{territoryId}");

			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();

		}

		public async Task<bool> TerritoryExists(string territoryId)
		{
			bool territoryExists = false;

			var response = await _httpClient.GetAsync($"{apiRoute}/getterritory");
			response.EnsureSuccessStatusCode();

			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
				territoryExists = JsonConvert.DeserializeObject<bool>(content.Result);

			return territoryExists;
		}

	}
}
