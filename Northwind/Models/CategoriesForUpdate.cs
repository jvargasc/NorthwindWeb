using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
	public partial class CategoriesForUpdate
	{
		[Required]
		public int CategoryId { get; set; }
		[Required, MaxLength(15)]
		public string CategoryName { get; set; }
		public string Description { get; set; }
		public byte[] Picture { get ; set ; }
	}
}
