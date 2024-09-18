using DonorService.DAL;
using DonorService.IRepository;
using DonorService.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace DonorService.Repository
{
    public class DonorRepository : IDonorRepository
    {
        private readonly DonorDbContext _context;

        public DonorRepository(DonorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Donor>> GetAllDonorsAsync()
        {
            return await _context.Donors.Include(d => d.BloodType)
                                        .Include(d => d.HealthStatus)
                                        .ToListAsync();
        }

        public async Task<Donor> GetDonorByIdAsync(int donorId)
        {
            return await _context.Donors.Include(d => d.BloodType)
                                        .Include(d => d.HealthStatus)
                                        .FirstOrDefaultAsync(d => d.DonorId == donorId);
        }

        public async Task AddDonorAsync(Donor donor)
        {
            await _context.Donors.AddAsync(donor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDonorAsync(Donor donor)
        {
            _context.Donors.Update(donor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDonorAsync(int donorId)
        {
            var donor = await _context.Donors.FindAsync(donorId);
            if (donor != null)
            {
                _context.Donors.Remove(donor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
