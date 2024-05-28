using Microsoft.EntityFrameworkCore;

namespace Web1.Models.DBModels
{
    [Keyless]public class ShipmentList
    {
        public int ShipmentID { get; set; }
        public string ProductName { get; set; }
        public int ProductCount { get; set; }
    }
}
