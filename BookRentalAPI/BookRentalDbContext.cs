using Microsoft.EntityFrameworkCore;
using BookRentalAPI.Models;

namespace BookRentalAPI.Data
{
    public class BookRentalDbContext : DbContext
    {
        public BookRentalDbContext(DbContextOptions<BookRentalDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Rentals)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@bookrental.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    FirstName = "Admin",
                    LastName = "User",
                    IsEmailConfirmed = true,
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    Email = "user@bookrental.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                    FirstName = "Regular",
                    LastName = "User",
                    IsEmailConfirmed = true
                }
            );

            // Seed books
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Description = "A story of wealth, love, and the American Dream in the 1920s.",
                    PublishedDate = new DateTime(1925, 4, 10),
                    IsAvailable = true
                },
                new Book
                {
                    Id = 2,
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Description = "A powerful story of racial injustice and moral growth in the American South.",
                    PublishedDate = new DateTime(1960, 7, 11),
                    IsAvailable = true
                }
            );

            // Seed reviews
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    BookId = 1,
                    UserId = 2,
                    Rating = 5,
                    ReviewText = "A timeless classic that captures the essence of the Jazz Age.",
                    ReviewDate = DateTime.UtcNow.AddDays(-10)
                },
                new Review
                {
                    Id = 2,
                    BookId = 2,
                    UserId = 2,
                    Rating = 4,
                    ReviewText = "A powerful narrative that remains relevant today.",
                    ReviewDate = DateTime.UtcNow.AddDays(-5)
                }
            );
        }
    }
}