using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Northwind.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Northwind.Services
{
    public class ServiceRegion: IDisposable
    {
        private readonly IConfiguration _configuration;
        private HttpClient _httpClient = new HttpClient();
        private const string apiRoute = "api/region";
        private const string mediaType = "application/json";

        public ServiceRegion(IConfiguration configuration)
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

		public async Task<List<Regions>> GetRegions(int page = 0, int itemsPerPage = 0)
        {
            List<Regions> region = new List<Regions>();

            var response = await _httpClient.GetAsync($"{apiRoute}/getregions?page={page.ToString()}&itemsPerPage={itemsPerPage.ToString()}");
			response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync();

            if (response.Content.Headers.ContentType.MediaType == mediaType)
            {
                region = JsonConvert.DeserializeObject<List<Regions>>(content.Result);
            }

            return region;
        }

        public async Task<Regions> GetRegion(int regionId)
        {
            Regions region = new Regions();

            var response = await _httpClient.GetAsync($"{apiRoute}/getregion/{regionId}");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync();

            if (response.Content.Headers.ContentType.MediaType == mediaType)
            {
                region = JsonConvert.DeserializeObject<Regions>(content.Result);
            }

            return region;
        }

        public async Task<RegionForCreation> CreateRegion(RegionForCreation regionToCreate)
        {
            var serializedRegionToCreate = JsonConvert.SerializeObject(regionToCreate);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiRoute}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            request.Content = new StringContent(serializedRegionToCreate);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdRegion = JsonConvert.DeserializeObject<RegionForCreation>(content);

            return createdRegion;

        }

        public async Task UpdateRegion(RegionForUpdate regionToUpdate)
        {
            var serializedRegionToUpdate = JsonConvert.SerializeObject(regionToUpdate);

            var request = new HttpRequestMessage(HttpMethod.Put, 
                $"{apiRoute}/{regionToUpdate.RegionId}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Content = new StringContent(serializedRegionToUpdate);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);
			 
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }

        public async Task DeleteRegion(int regionId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
                $"{apiRoute}/{regionId}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            var respose = await _httpClient.SendAsync(request);
            respose.EnsureSuccessStatusCode();
        }

        public async Task<bool> RegionExists(int regionId)
        {
            bool regionExists = false;

            var response = await _httpClient.GetAsync($"{apiRoute}/getregion");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync();

            if (response.Content.Headers.ContentType.MediaType == mediaType)
                regionExists = JsonConvert.DeserializeObject<bool>(content.Result);

            return regionExists;
        }

		public void Dispose()
		{
			//Dispose(true);
			GC.SuppressFinalize(this);
		}

		//protected virtual void Dispose(bool disposing)
		//{
		//	if (disposing)
		//	{
		//		if (_context != null)
		//		{
		//			_context.Dispose();
		//			_context = null;
		//		}
		//	}
		//}
	}
}
