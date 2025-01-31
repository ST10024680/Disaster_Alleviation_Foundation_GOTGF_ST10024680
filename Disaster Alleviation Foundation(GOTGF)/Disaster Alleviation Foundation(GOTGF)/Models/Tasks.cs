using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Disaster_Alleviation_Foundation_GOTGF_.Models
{
    public class Tasks
    {
        [Key]
        public int TaskID { get; set; }

        
        public string Title { get; set; }

       
        public string Description { get; set; }

        public DateTime ScheduledDate { get; set; }
        

        
        
    }
}
