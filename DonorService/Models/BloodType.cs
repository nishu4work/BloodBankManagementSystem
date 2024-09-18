namespace DonorService.Models
{
    public class BloodType
    {
        public int BloodTypeId { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Donor> Donors { get; set; }

    }
}
