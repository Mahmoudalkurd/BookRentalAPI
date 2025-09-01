using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookRentalAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public bool IsEmailConfirmed { get; set; } = false;
        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = "User";

        // Navigation properties
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}