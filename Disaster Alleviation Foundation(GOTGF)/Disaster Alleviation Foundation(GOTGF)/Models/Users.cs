using System.ComponentModel.DataAnnotations;

namespace Disaster_Alleviation_Foundation_GOTGF_.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
