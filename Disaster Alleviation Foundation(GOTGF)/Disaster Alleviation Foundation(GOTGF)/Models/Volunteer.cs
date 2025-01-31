using System.ComponentModel.DataAnnotations;

namespace Disaster_Alleviation_Foundation_GOTGF_.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerID { get; set; }

        
        public string VName { get; set; }

        
        public string Email { get; set; }

        
        public string VPhoneNumber { get; set; }
       
    }
}
