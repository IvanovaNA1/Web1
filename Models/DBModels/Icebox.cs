using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class Icebox
    {
        [Key]public int IceboxID { get; set; }
        public int WarehouseID { get; set; }
    }
}
