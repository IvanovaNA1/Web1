using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class UserRole
    {
        [Key]public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
