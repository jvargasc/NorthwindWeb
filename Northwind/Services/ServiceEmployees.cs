using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Northwind.Models;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;

namespace Northwind.Services
{
	public class ServiceEmployees
	{
		private IConfiguration _configuration { get; }
		private HttpClient _httpClient = new HttpClient();
        private const string apiRoute = "api/employees";
        private const string mediaType = "application/json";

        public ServiceEmployees(IConfiguration configuration)
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

		public async Task<List<Employees>> GetEmployees(int page = 0, int itemsPerPage = 0)
		{
			List<Employees> employees = new List<Employees>();

			var response = await _httpClient.GetAsync($"{apiRoute}/getemployees?page={page.ToString()}&itemsPerPage={itemsPerPage.ToString()}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();
			
			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				employees = JsonConvert.DeserializeObject<List<Employees>>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(List<Employees>));
				employees = (List<Employees>)serializer.Deserialize(new StringReader(content.Result));
			}

			return employees;
		}

		public async Task<Employees> GetEmployee(int employeeId)
		{
			Employees employee = new Employees();

			var response = await _httpClient.GetAsync($"{apiRoute}/getemployee/{employeeId}");
			response.EnsureSuccessStatusCode();
			var content = response.Content.ReadAsStringAsync();

			if (response.Content.Headers.ContentType.MediaType == "application/json")
			{
				employee = JsonConvert.DeserializeObject<Employees>(content.Result);
			}
			else if (response.Content.Headers.ContentType.MediaType == "application/xml")
			{
				var serializer = new XmlSerializer(typeof(Employees));
				employee = (Employees)serializer.Deserialize(new StringReader(content.Result));
			}

			return employee;
		}

        public async Task<EmployeesForCreation> CreateEmployee(EmployeesForCreation employeeToCreate)
        {
            var serializedEmployeeToCreate = JsonConvert.SerializeObject(employeeToCreate);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiRoute}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            request.Content = new StringContent(serializedEmployeeToCreate);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdEmployee = JsonConvert.DeserializeObject<EmployeesForCreation>(content);

            return createdEmployee;
        }

        public async Task UpdateEmployee(EmployeesForUpdate employeeToUpdate)
        {
            var serializedEmployeeToUpdate = JsonConvert.SerializeObject(employeeToUpdate);

            var request = new HttpRequestMessage(HttpMethod.Put,
                $"{apiRoute}/{employeeToUpdate.EmployeeId}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Content = new StringContent(serializedEmployeeToUpdate);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
        }

        public async Task DeleteEmployee(int employeeId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
                $"{apiRoute}/{employeeId}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> EmployeeExists(int employeeId)
        {
            bool employeeExists = false;

            var response = await _httpClient.GetAsync($"{apiRoute}/getemployee/{employeeId}");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync();

            if (response.Content.Headers.ContentType.MediaType == mediaType)
                employeeExists = JsonConvert.DeserializeObject<bool>(content.Result);

            return employeeExists;
        }

	}
}
