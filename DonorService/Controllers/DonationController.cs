using DonorService.DTO;
using DonorService.IRepository;
using DonorService.Models;
using DonorService.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonorService.Controllers
{
    public class DonationController : Controller
    {
        private readonly IDonationRepository _donationRepository;

        public DonationController(IDonationRepository donationRepository)
        {
            _donationRepository = donationRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("GetCertificate/{donationId}")]
        public async Task<IActionResult> GetCertificate(int donationId)
        {
           // Return Certificate
            return Ok("Donor registered successfully.");
        }

        [Authorize(Roles = "Admin,Donor")]
        [HttpGet("DonationHistory/{donorId}")]
        public async Task<IActionResult> GetDonationHistory(int donorId)
        {
            var donationHistory = await _donationRepository.GetDonationHistoryByDonorIdAsync(donorId);
            return Ok(donationHistory);
        }

    }
}
