using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class ProductTypes
    {
        public int Id { set; get; }
        [Required]
        [Display(Name ="Product Type")]
        public string ProductType { set; get; }
    }

    public class SerialTags
    {
        public int Id { set; get; }
        [Required]
        [Display(Name = "Serial Tag")]
        public string SerialTag { set; get; }
    }

    public class Products
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public decimal Price { set; get; }
        public string Image { set; get; }
        [Required]
        [Display(Name ="Product Color")]
        public string ProductColor { set; get; }
        [Display(Name ="Availbale")]
        public bool  IsAvailable { set; get; }
        [Required]
        [Display(Name = "Product Type")]
        public int ProductTypesId { set; get; }
        [ForeignKey("ProductTypesId")]
        public ProductTypes ProductTypes { set; get; }
        [Required]
        [Display(Name = "Serial Tag")]
        public int SerialTagsId { set; get; }
        [ForeignKey("SerialTagsId")]
        public SerialTags SerialTags { set; get; }
    }
}
