using AutoMapper;
using BookRentalAPI.DTOs;
using BookRentalAPI.Models;
using Microsoft.EntityFrameworkCore;
using BookRentalAPI.Data;

namespace BookRentalAPI.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetReviewsByBook(int bookId);
        Task<ReviewDto> AddReview(CreateReviewDto createReviewDto, int userId);
        Task<bool> DeleteReview(int id, int userId);
    }

    public class ReviewService : IReviewService
    {
        private readonly BookRentalDbContext _context;
        private readonly IMapper _mapper;

        public ReviewService(BookRentalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByBook(int bookId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.BookId == bookId)
                .ToListAsync();

            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                Rating = r.Rating,
                ReviewText = r.ReviewText,
                ReviewDate = r.ReviewDate,
                UserName = $"{r.User.FirstName} {r.User.LastName}"
            });
        }

        public async Task<ReviewDto> AddReview(CreateReviewDto createReviewDto, int userId)
        {
            var book = await _context.Books.FindAsync(createReviewDto.BookId);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            var review = new Review
            {
                BookId = createReviewDto.BookId,
                UserId = userId,
                Rating = createReviewDto.Rating,
                ReviewText = createReviewDto.ReviewText,
                ReviewDate = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userId);
            return new ReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                ReviewDate = review.ReviewDate,
                UserName = $"{user.FirstName} {user.LastName}"
            };
        }

        public async Task<bool> DeleteReview(int id, int userId)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                throw new Exception("Review not found");
            }

            if (review.UserId != userId)
            {
                throw new Exception("You can only delete your own reviews");
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}