using Microsoft.EntityFrameworkCore;

namespace Web1.Models.DBModels
{
    [Keyless] public class OrderServices
    {
        public int OrderID { get; set; }
        public string ServiceName { get; set; }
        public int ServiceCount { get; set; }
    }
}
