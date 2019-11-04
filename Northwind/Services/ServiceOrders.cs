using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Northwind.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Northwind.Services
{
	public class ServiceOrders
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();
        private const string apiRoute = "api/orders";
        private const string mediaType = "application/json";

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

		public ServiceOrders(IConfiguration configuration)
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

		public async Task<List<Orders>> GetOrders(int page = 0, int itemsPerPage = 0)
		{
			List<Orders> orders = new List<Orders>();

			var response = await _httpClient.GetAsync($"{apiRoute}/getorders?page={page.ToString()}&itemsPerPage={itemsPerPage.ToString()}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				orders = JsonConvert.DeserializeObject<List<Orders>>(content.Result);
			}

			return orders;
		}

		public async Task<Orders> GetOrder(int orderId)
		{
			Orders order = new Orders();

			var response = await _httpClient.GetAsync($"{apiRoute}/getorder/{orderId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				order = JsonConvert.DeserializeObject<Orders>(content.Result);
			}

			return order;
		}

        public async Task<OrdersForCreation> CreateOrder(OrdersForCreation orderToCreate)
        {
            var serializedOrderToCreate = JsonConvert.SerializeObject(orderToCreate);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiRoute}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            request.Content = new StringContent(serializedOrderToCreate);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdOrder = JsonConvert.DeserializeObject<OrdersForCreation>(content);

            return createdOrder;

        }

        public async Task UpdateOrder(OrdersForUpdate orderToUpdate)
        {
            var serializedOrderToUpdate = JsonConvert.SerializeObject(orderToUpdate);

            var request = new HttpRequestMessage(HttpMethod.Put,
                $"{apiRoute}/{orderToUpdate.OrderId}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Content = new StringContent(serializedOrderToUpdate);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }

        public async Task DeleteOrder(int orderId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
                $"{apiRoute}/{orderId}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> OrderExists(int orderId)
        {
            bool orderExists = false;

            var response = await _httpClient.GetAsync($"{apiRoute}/getorder/{orderId}");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync();

            if (response.Content.Headers.ContentType.MediaType == mediaType)
                orderExists = JsonConvert.DeserializeObject<bool>(content.Result);

            return orderExists;
        }
	}
}
