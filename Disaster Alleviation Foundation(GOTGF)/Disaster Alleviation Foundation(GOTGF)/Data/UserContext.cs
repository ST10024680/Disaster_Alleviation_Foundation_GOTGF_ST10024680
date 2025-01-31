using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Disaster_Alleviation_Foundation_GOTGF_.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
        : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<IncidentReports> IncidentReports { get; set; }
        
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Volunteer> Volunteer { get; set; }
        public DbSet<ResourceDonation> ResourceDonation { get; set; }

    }
}
