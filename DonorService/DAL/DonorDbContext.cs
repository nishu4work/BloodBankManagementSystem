using DonorService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DonorService.DAL
{
    public class DonorDbContext : DbContext
    {
        public DonorDbContext(DbContextOptions<DonorDbContext> options) : base(options)
        {
        }

        // DbSets for each entity
        public DbSet<Donor> Donors { get; set; }
        public DbSet<BloodType> BloodTypes { get; set; }
        public DbSet<HealthStatus> HealthStatuses { get; set; }
        public DbSet<BloodDonation> Donations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // Configure Donor relationships
            modelBuilder.Entity<Donor>(entity =>
            {
                entity.HasOne(d => d.BloodType)
               .WithMany(bt => bt.Donors)
               .HasForeignKey(d => d.BloodTypeId);

                entity.HasOne(d => d.HealthStatus)
                .WithMany(hs => hs.Donors)
                .HasForeignKey(d => d.HealthStatusId);
            });

            // Configure Donation relationships
            modelBuilder.Entity<BloodDonation>()
                .HasOne(d => d.Donor)
                .WithMany(dn => dn.BloodDonations)
                .HasForeignKey(d => d.DonorId);


        }
    }
}
