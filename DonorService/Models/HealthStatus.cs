namespace DonorService.Models
{
    public class HealthStatus
    {
        public int HealthStatusId { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Donor> Donors { get; set; }

    }
}
