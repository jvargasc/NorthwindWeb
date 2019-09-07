using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API
{
	public class Startup
	{

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors((p) => {
				p.AddDefaultPolicy((x) => {
					x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
				});
			});

			services.AddMvc().AddMvcOptions(o => o.OutputFormatters.Add(
				new XmlDataContractSerializerOutputFormatter())
			)
			.AddJsonOptions(o =>
			{
				if (o.SerializerSettings.ContractResolver != null)
				{
					var castedResolver = o.SerializerSettings.ContractResolver
						as DefaultContractResolver;
					castedResolver.NamingStrategy = null;
				}
			});

			string ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariables()["ASPNETCORE_ENVIRONMENT"].ToString();
			string connectionString;

			if (ASPNETCORE_ENVIRONMENT == "Development")
				connectionString = Environment.GetEnvironmentVariables()["connectionStrings::northwindwebdbconnectionstring"].ToString();
			else
				connectionString = Environment.GetEnvironmentVariables()["SQLAZURECONNSTR_northwindwebdbconnectionstring"].ToString();

			services.AddDbContext<NorthwindContext>(o => o.UseSqlServer(connectionString));

			services.AddScoped<ICategoriesRepository, CategoriesRepository>();
			services.AddScoped<ICustomersRepository, CustomersRepository>();
			services.AddScoped<IEmployeesRepository, EmployeesRepository>();
			services.AddScoped<IOrdersRepository, OrdersRepository> ();
			services.AddScoped<IProductsRepository, ProductsRepository> ();
			services.AddScoped<IRegionRepository, RegionRepository> ();
			services.AddScoped<IShippersRepository, ShippersRepository> ();
			services.AddScoped<ISuppliersRepository, SuppliersRepository> ();
			services.AddScoped<ITerritoriesRepository,  TerritoriesRepository> ();

			services.AddAutoMapper();
			services.AddSwaggerDocument();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env,
					 ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseOpenApi();
			app.UseSwaggerUi3();

			app.UseStatusCodePages();

			app.UseCors();
			app.UseHttpsRedirection();

			app.UseMvc();

			//app.Run(async (context) =>
			//{
			//	//await context.Response.WriteAsync("Hello World!");
			//	await context.Response.WriteAsync("Welcome to Northwind.API!!!");
			//});
		}
	}
}