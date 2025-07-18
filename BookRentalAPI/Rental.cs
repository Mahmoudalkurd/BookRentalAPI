using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookRentalAPI.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime RentalDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ReturnDate { get; set; }

        public DateTime? ActualReturnDate { get; set; }

        // Foreign keys
        public int BookId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}