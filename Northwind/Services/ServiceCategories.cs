using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Northwind.Models;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using System.Net.Http.Headers;
 
namespace Northwind.Services
{
	public class ServiceCategories
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();
        private const string apiRoute = "api/categories";
        private const string mediaType = "application/json";

        public ServiceCategories(IConfiguration configuration)
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
		public async Task<List<Categories>> GetCategories(int page = 0, int itemsPerPage = 0)
		{
			List<Categories> categories = new List<Categories>();

			var response = await _httpClient.GetAsync($"{apiRoute}/getcategories?page={page.ToString()}&itemsPerPage={itemsPerPage.ToString()}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				categories = JsonConvert.DeserializeObject<List<Categories>>(content.Result);
			}

			return categories;
		}
		public async Task<Categories> GetCategory(int categoryId)
		{
			Categories category = new Categories();

			var response = await _httpClient.GetAsync($"{apiRoute}/getcategory/{categoryId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				category = JsonConvert.DeserializeObject<Categories>(content.Result);
			}

			return category;
		}
		public async Task<CategoriesForCreation> CreateCategory(CategoriesForCreation categoryToCreate)
		{
			var serializedCategoryToCreate = JsonConvert.SerializeObject(categoryToCreate);

			var request = new HttpRequestMessage(HttpMethod.Post, $"{apiRoute}");
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

			request.Content = new StringContent(serializedCategoryToCreate);
			request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var createdCategory = JsonConvert.DeserializeObject<CategoriesForCreation>(content);

			return createdCategory;

		}
		public async Task UpdateCategory(CategoriesForUpdate categoryToUpdate)
		{
			var serializedCustomerToUpdate = JsonConvert.SerializeObject(categoryToUpdate);

			var request = new HttpRequestMessage(HttpMethod.Put,
                $"{apiRoute}/{categoryToUpdate.CategoryId}");

			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
			request.Content = new StringContent(serializedCustomerToUpdate);
			request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();

		}
		public async Task DeleteCategory(int categoryId)
		{
			var request = new HttpRequestMessage(HttpMethod.Delete,
				$"api/categories/{categoryId}");
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
		}

		public async Task<bool> CategoryExists(int categoryId)
		{
			bool categoryExists = false;

			var response = await _httpClient.GetAsync($"{apiRoute}/getcategory/{categoryId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				categoryExists = JsonConvert.DeserializeObject<bool>(content.Result);
			}

			return categoryExists;
		}
	}
}
