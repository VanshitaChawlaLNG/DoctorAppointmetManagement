using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DoctorAppointmentManagement.Models;
using DoctorAppointmentManagement.Contracts;
using Microsoft.AspNetCore.Http;

namespace DoctorAppointmentManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<TimingSlots> TimingSlots { get; set; }
        public DbSet<AvailableTiming> AvailableTimings { get; set; }
        public IFormFile ImageFile { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between Doctor and Timing

            
              modelBuilder.Entity<TimingSlots>()
                .HasOne(t => t.Doctor)
                .WithMany(d => d.timingSlots)
                .HasForeignKey(t => t.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TimeSlot>()
              .HasOne(ts => ts.TimingSlots)
              .WithMany(ts => ts.Slots)
              .HasForeignKey(ts => ts.TimingSlotsId)
              .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
