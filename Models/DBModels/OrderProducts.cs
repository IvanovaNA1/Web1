using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class OrderProducts
    {
        [Key] public int OrderID { get; set; }
        public string ProductName { get; set; }
        public int ProductCount { get; set; }
    }
}
