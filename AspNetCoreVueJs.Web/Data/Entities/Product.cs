using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace  AspNetCoreVueJs.Web.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        [Required]
        public string Thumbnail { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string Description { get; set; }
       // [Required]
       // public decimal Price { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ScreenSize { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TalkTime { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal StandbyTime { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int OSId { get; set; }

        public List<Image> Images { get; set; }
        public Brand Brand { get; set; }
        public OS OS { get; set; }
        public List<ProductFeature> ProductFeatures { get; set; } =
        new List<ProductFeature>();
        public List<ProductVariant> ProductVariants { get; set; } =
    new List<ProductVariant>();
    }
}
