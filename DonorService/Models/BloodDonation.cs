namespace DonorService.Models
{
    public class BloodDonation
    {
        public int BloodDonationId { get; set; }
        public int DonorId { get; set; }
        public int BloodTypeId { get; set; }
        public DateTime DonationDate { get; set; }

        public virtual Donor Donor { get; set; }
        public virtual BloodType BloodType { get; set; }


    }
}
