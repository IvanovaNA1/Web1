using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class OrderServices
    {
        [Key]public int OrderServicesID { get; set; }
        public int OrderID { get; set; }
        public string ServiceName { get; set; }
        public int ServiceCount { get; set; }
    }
}
