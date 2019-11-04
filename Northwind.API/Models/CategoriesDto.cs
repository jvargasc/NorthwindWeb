
namespace Northwind.API.Models
{
	public partial class CategoriesDto
    {
		//public CategoriesDto()
		//{
		//Products = new HashSet<Products>();
		//}

		public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
		public byte[] Picture { get; set; }

		//public ICollection<Products> Products { get; set; }

	}
}
