using AutoMapper;
using BookRentalAPI.DTOs;
using BookRentalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookRentalAPI.Services
{
    public interface IRentalService
    {
        Task<IEnumerable<RentalDto>> GetUserRentals(int userId);
        Task<RentalDto> RentBook(CreateRentalDto createRentalDto, int userId);
        Task<bool> ReturnBook(int rentalId, int userId);
    }

    public class RentalService : IRentalService
    {
        private readonly BookRentalDbContext _context;
        private readonly IMapper _mapper;

        public RentalService(BookRentalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RentalDto>> GetUserRentals(int userId)
        {
            var rentals = await _context.Rentals
                .Include(r => r.Book)
                .Where(r => r.UserId == userId && r.ActualReturnDate == null)
                .ToListAsync();

            return rentals.Select(r => new RentalDto
            {
                Id = r.Id,
                RentalDate = r.RentalDate,
                ReturnDate = r.ReturnDate,
                ActualReturnDate = r.ActualReturnDate,
                BookTitle = r.Book.Title
            });
        }

        public async Task<RentalDto> RentBook(CreateRentalDto createRentalDto, int userId)
        {
            var book = await _context.Books.FindAsync(createRentalDto.BookId);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            if (!book.IsAvailable)
            {
                throw new Exception("Book is not available for rent");
            }

            var rental = new Rental
            {
                BookId = createRentalDto.BookId,
                UserId = userId,
                RentalDate = DateTime.UtcNow,
                ReturnDate = DateTime.UtcNow.AddDays(createRentalDto.RentalDays)
            };

            book.IsAvailable = false;
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return new RentalDto
            {
                Id = rental.Id,
                RentalDate = rental.RentalDate,
                ReturnDate = rental.ReturnDate,
                BookTitle = book.Title,
                UserName = (await _context.Users.FindAsync(userId))?.Email
            };
        }

        public async Task<bool> ReturnBook(int rentalId, int userId)
        {
            var rental = await _context.Rentals
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == rentalId && r.UserId == userId);

            if (rental == null)
            {
                throw new Exception("Rental not found or you don't have permission");
            }

            if (rental.ActualReturnDate != null)
            {
                throw new Exception("Book already returned");
            }

            rental.ActualReturnDate = DateTime.UtcNow;
            rental.Book.IsAvailable = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}