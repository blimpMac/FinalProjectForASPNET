namespace FinalProject.DTOs
{
    public class BookDTO
    {
        public string ISBNNumber { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public DateOnly DatePublished { get; set; }
        public int? PrintedYear { get; set; }
        public string PrintedBy { get; set; }
        public string Availability { get; set; }
    }
}
