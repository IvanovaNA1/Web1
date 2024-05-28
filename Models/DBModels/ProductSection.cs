using Microsoft.EntityFrameworkCore;

namespace Web1.Models.DBModels
{
    [Keyless]public class ProductSection
    {
        public string ProductName { get; set; }
        public int SectionID { get; set; }
    }
}
