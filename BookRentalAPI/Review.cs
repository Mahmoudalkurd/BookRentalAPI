using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookRentalAPI.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        public string ReviewText { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

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