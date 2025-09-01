using AutoMapper;
using BookRentalAPI.DTOs;
using BookRentalAPI.Models;
using Microsoft.EntityFrameworkCore;
using BookRentalAPI.Data;

namespace BookRentalAPI.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooks(string filter = null, string sort = "asc");
        Task<BookDto> GetBookById(int id);
        Task<BookDto> AddBook(CreateBookDto createBookDto);
        Task<BookDto> UpdateBook(int id, CreateBookDto createBookDto);
        Task<bool> DeleteBook(int id);
    }

    public class BookService : IBookService
    {
        private readonly BookRentalDbContext _context;
        private readonly IMapper _mapper;

        public BookService(BookRentalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooks(string filter = null, string sort = "asc")
        {
            var query = _context.Books.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(b => 
                    b.Title.Contains(filter) || 
                    b.Author.Contains(filter) ||
                    b.Description.Contains(filter));
            }

            // Sorting
            query = sort.ToLower() == "desc" 
                ? query.OrderByDescending(b => b.Title) 
                : query.OrderBy(b => b.Title);

            var books = await query.ToListAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            // Calculate average rating
            var averageRating = await _context.Reviews
                .Where(r => r.BookId == id)
                .AverageAsync(r => (double?)r.Rating);

            var bookDto = _mapper.Map<BookDto>(book);
            bookDto.AverageRating = averageRating ?? 0;
            return bookDto;
        }

        public async Task<BookDto> AddBook(CreateBookDto createBookDto)
        {
            var book = _mapper.Map<Book>(createBookDto);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateBook(int id, CreateBookDto createBookDto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            _mapper.Map(createBookDto, book);
            await _context.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}