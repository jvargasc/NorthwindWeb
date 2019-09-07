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

		public async Task<List<Categories>> GetCategories()
		{
			List<Categories> categories = new List<Categories>();

			var response = await _httpClient.GetAsync("api/categories/getcategories");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				categories = JsonConvert.DeserializeObject<List<Categories>>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(List<Categories>));
				categories = (List<Categories>)serializer.Deserialize(new StringReader(content.Result));
			}

			return categories;
		}
		public async Task<Categories> GetCategory(int categoryId)
		{
			Categories category = new Categories();

			var response = await _httpClient.GetAsync($"api/categories/getcategory/{categoryId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				category = JsonConvert.DeserializeObject<Categories>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(Categories));
				category = (Categories)serializer.Deserialize(new StringReader(content.Result));
			}

			return category;
		}
	}
}
