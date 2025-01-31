using System.ComponentModel.DataAnnotations;

namespace Disaster_Alleviation_Foundation_GOTGF_.Models
{
    public class ResourceDonation
    {
        [Key]
        public int DonationID { get; set; } 

        public string ResourceType { get; set; } 
        public string Description { get; set; } 
        public int Quantity { get; set; } 
        public DateTime DonationDate { get; set; } 
        public bool IsDistributed { get; set; } 
    }
}
