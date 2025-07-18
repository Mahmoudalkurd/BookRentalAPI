namespace BookRentalAPI.DTOs
{
    public class RentalDto
    {
        public int Id { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public string BookTitle { get; set; }
        public string UserName { get; set; }
    }

    public class CreateRentalDto
    {
        public int BookId { get; set; }
        public int RentalDays { get; set; }
    }
}