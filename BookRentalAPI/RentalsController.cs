using BookRentalAPI.DTOs;
using BookRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookRentalAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet("my-rentals")]
        public async Task<IActionResult> GetUserRentals()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var rentals = await _rentalService.GetUserRentals(userId);
            return Ok(rentals);
        }

        [HttpPost]
        public async Task<IActionResult> RentBook(CreateRentalDto createRentalDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var rental = await _rentalService.RentBook(createRentalDto, userId);
                return CreatedAtAction(nameof(GetUserRentals), rental);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("return/{rentalId}")]
        public async Task<IActionResult> ReturnBook(int rentalId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _rentalService.ReturnBook(rentalId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}