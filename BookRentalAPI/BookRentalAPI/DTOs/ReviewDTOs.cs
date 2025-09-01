namespace BookRentalAPI.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public string UserName { get; set; }
    }

    public class CreateReviewDto
    {
        public int BookId { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
    }
}