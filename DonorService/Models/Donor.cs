namespace DonorService.Models
{
    public class Donor
    {
        public int DonorId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int BloodTypeId { get; set; }  // Foreign Key from BloodType entity
        public string Phone { get; set; }
        public int HealthStatusId { get; set; }  // Foreign Key from HealthStatus entity
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual BloodType BloodType { get; set; }
        public virtual HealthStatus HealthStatus { get; set; }
        public virtual ICollection<BloodDonation> BloodDonations { get; set; }

    }
}
