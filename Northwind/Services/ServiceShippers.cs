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
	public class ServiceShippers
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();
        private const string apiRoute = "api/shippers";
        private const string mediaType = "application/json";

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

		public async Task<List<Shippers>> GetShippers(int page = 0, int itemsPerPage = 0)
		{
			List<Shippers> shippers = new List<Shippers>();

			var response = await _httpClient.GetAsync($"{apiRoute}/getshippers?page={page.ToString()}&itemsPerPage={itemsPerPage.ToString()}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				shippers = JsonConvert.DeserializeObject<List<Shippers>>(content.Result);
			}

			return shippers;
		}

		public async Task<Shippers> GetShipper(int shipperId)
		{
			Shippers shipper = new Shippers();

			var response = await _httpClient.GetAsync($"{apiRoute}/getshipper/{shipperId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				shipper = JsonConvert.DeserializeObject<Shippers>(content.Result);
			}

			return shipper;
		}

        public async Task<ShippersForCreation> CreateShipper(ShippersForCreation shippersToCreate)
        {
            var serializedShippersToCreate = JsonConvert.SerializeObject(shippersToCreate);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiRoute}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            request.Content = new StringContent(serializedShippersToCreate);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdShipper = JsonConvert.DeserializeObject<ShippersForCreation>(content);

            return createdShipper;
        }

        public async Task UpdateShipper(ShippersForUpdate shipperToUpdate)
        {
            var serializedShipperToUpdate = JsonConvert.SerializeObject(shipperToUpdate);

            var request = new HttpRequestMessage(HttpMethod.Put,
                $"{apiRoute}/{shipperToUpdate.ShipperId}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Content = new StringContent(serializedShipperToUpdate);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }

        public async Task DeleteShipper(int shipperId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
                $"{apiRoute}/{shipperId}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }

        public async Task<bool> ShipperExists(int shipperId)
        {
            bool shipperExists = false;

            var response = await _httpClient.GetAsync($"{apiRoute}/getshipper");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync();

            if (response.Content.Headers.ContentType.MediaType == mediaType)
                shipperExists = JsonConvert.DeserializeObject<bool>(content.Result);

            return shipperExists;
        }

	}
}
