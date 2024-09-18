using DonorService.Models;
using System.Drawing;

namespace DonorService.IRepository
{
    public interface IDonationRepository
    {
        Task<IEnumerable<BloodDonation>> GetDonationHistoryByDonorIdAsync(int donorId);
    }

}
