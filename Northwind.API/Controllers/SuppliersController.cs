using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/suppliers")]
    public class SuppliersController : Controller
    {
		private readonly ISuppliersRepository _suppliersRepository;
		private readonly IMapper _mapper;

		public SuppliersController(ISuppliersRepository suppliersRepository,
								   IMapper mapper)
		{
			_suppliersRepository = suppliersRepository;
			_mapper = mapper;
		}

		[HttpGet("getsuppliers")]
        public async Task<ActionResult> GetSuppliers()
        {
			var suppliersEntities = await _suppliersRepository.GetSuppliers();
			var _results = _mapper.Map<IEnumerable<SuppliersDto>>(suppliersEntities);
			//var _results = new List<SuppliersDto>();

			//foreach (var item in suppliersEntities)
			//{
			//	_results.Add(new SuppliersDto
			//	{
			//		SupplierId = item.SupplierId,
			//		CompanyName = item.CompanyName,
			//		ContactName = item.ContactName,
			//		ContactTitle = item.ContactTitle,
			//		Address = item.Address,
			//		City = item.City,
			//		Region = item.Region,
			//		PostalCode = item.PostalCode,
			//		Country = item.Country,
			//		Phone = item.Phone,
			//		Fax = item.Fax,
			//		HomePage = item.HomePage
			//	});
			//}

			return Ok(_results);
        }

		[HttpGet("getsupplier/{supplierId}")]
		public async Task<ActionResult> GetSupplier(int supplierId)
		{
			var suppliersEntity = await _suppliersRepository.GetSupplier(supplierId);
			var _results = _mapper.Map<Suppliers>(suppliersEntity);

			return Ok(_results);
		}

	}
}