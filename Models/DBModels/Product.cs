    using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace Web1.Models.DBModels
{
    public class Product
    {
        [Key]
        public string? ProductName { get; set; }
        public Category ProductCategory { get; set; }
        //public string? ProductCategory { get; set; }
        public int ProductProvider { get; set; }
        public string? ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductNum { get; set; }
    }
}
