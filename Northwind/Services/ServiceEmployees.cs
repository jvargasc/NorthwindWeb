using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

		public async Task<List<Employees>> GetEmployees()
		{
			List<Employees> employees = new List<Employees>();

			var response = await _httpClient.GetAsync("api/employees/getemployees");
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

			var response = await _httpClient.GetAsync($"api/employees/getemployee/{employeeId}");
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
	}
}
