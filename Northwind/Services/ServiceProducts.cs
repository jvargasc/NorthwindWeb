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
	public class ServiceProducts
	{
		private readonly IConfiguration _configuration;
		private HttpClient _httpClient = new HttpClient();
        private const string apiRoute = "api/products";
        private const string mediaType = "application/json";

        public ServiceProducts(IConfiguration configuration)
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

		public async Task<List<Products>> GetProducts(int page = 0, int itemsPerPage = 0)
		{
			List<Products> products = new List<Products>();

			var response = await _httpClient.GetAsync($"{apiRoute}/getproducts?page={page.ToString()}&itemsPerPage={itemsPerPage.ToString()}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				products = JsonConvert.DeserializeObject<List<Products>>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(List<Products>));
				products = (List<Products>)serializer.Deserialize(new StringReader(content.Result));
			}

			return products;
		}
		
		public async Task<Products> GetProduct(int productId)
		{
			Products product = new Products();

			var response = await _httpClient.GetAsync($"{apiRoute}/getproduct/{productId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == mediaType)
			{
				product = JsonConvert.DeserializeObject<Products>(content.Result);
			}

			return product;
		}

        public async Task<ProductsForCreation> CreateProduct (ProductsForCreation productToCreate)
        {
            var serializedproductToCreate = JsonConvert.SerializeObject(productToCreate);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiRoute}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            request.Content = new StringContent(serializedproductToCreate);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdProduct = JsonConvert.DeserializeObject<ProductsForCreation>(content);

            return createdProduct;

        }

        public async Task UpdateProduct(ProductsForUpdate productToUpdate)
        {
            var serializedProductToUpdate = JsonConvert.SerializeObject(productToUpdate);

            var request = new HttpRequestMessage(HttpMethod.Put,
                $"{apiRoute}/{productToUpdate.ProductId}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Content = new StringContent(serializedProductToUpdate);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }

        public async Task DeleteProduct(int productId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
                $"{apiRoute}/{productId}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }

        public async Task<bool> ProductExists(int productId)
        {
            bool productExists = false;

            var response = await _httpClient.GetAsync($"{apiRoute}/getproduct");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync();

            if (response.Content.Headers.ContentType.MediaType == mediaType)
                productExists = JsonConvert.DeserializeObject<bool>(content.Result);

            return productExists;
        }
	}
}
