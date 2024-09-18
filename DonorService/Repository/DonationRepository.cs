using DonorService.DAL;
using DonorService.IRepository;
using DonorService.Models;
using Microsoft.EntityFrameworkCore;

namespace DonorService.Repository
{
    public class DonationRepository : IDonationRepository
    {
        private readonly DonorDbContext _context;

        public DonationRepository(DonorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BloodDonation>> GetDonationHistoryByDonorIdAsync(int donorId)
        {
            return await _context.Donations.Where(d => d.DonorId == donorId).ToListAsync();
        }

      
    }
}
