using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  AspNetCoreVueJs.Web.Data.Entities
{
    public class ProductVariant
    {
        public int ProductId { get; set; }
        public int ColourId { get; set; }
        public int StorageId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public Product Product { get; set; }
        public Colour Colour { get; set; }
        public Storage Storage { get; set; }
    }
}
