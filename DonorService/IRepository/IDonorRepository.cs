using DonorService.Models;

namespace DonorService.IRepository
{
    public interface IDonorRepository
    {
        Task<IEnumerable<Donor>> GetAllDonorsAsync();
        Task<Donor> GetDonorByIdAsync(int donorId);
        Task AddDonorAsync(Donor donor);
        Task UpdateDonorAsync(Donor donor);
        Task DeleteDonorAsync(int donorId);
    }
}
