using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace Web1.Models.DBModels
{
    public class Service
    {
        [Key]
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public decimal ServicePrice { get; set; }
    }
}
