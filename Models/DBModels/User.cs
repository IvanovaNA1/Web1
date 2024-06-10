using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class User
    {
        [Key] public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? UserPhone { get; set; }
        public string? UserLogin { get; set; }
        public string? UserPassword { get; set; }
        public int RoleID { get; set; }
        public UserRole Role { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
