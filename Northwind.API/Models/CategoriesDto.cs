
namespace Northwind.API.Models
{
	public partial class CategoriesDto
    {
        public CategoriesDto()
        {
            //Products = new HashSet<Products>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
		public byte[] Picture { set { _picture = value; } }
		private byte[] _picture;
		public string Picture64
		{
			get { return Utilities.Base64String(_picture); }
		}

		//public ICollection<Products> Products { get; set; }

	}
}
