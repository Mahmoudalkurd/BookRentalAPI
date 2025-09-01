using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookRentalAPI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Navigation properties
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}