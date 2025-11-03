namespace FinalProject.DTOs
{
    public class BookDTO_ReadDTO
    {
        public string ISBNNumber { get; set; } = null!;
        public string BookName { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public DateOnly DatePublished { get; set; }
        public int PrintedYear { get; set; }
        public string PrintedBy { get; set; } = null!;
        public string Availability { get; set; } = null!;
    }

    public class BookDTO_CreateDTO
    {
        public string ISBNNumber { get; set; } = null!;
        public string BookName { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
    }

    public class BookDTO_UpdateDTO
    {
        public string BookName { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
    }

    public class BookDTO_DeleteDTO
    {
        public string ISBNNumber { get; set; } = null!;
    }
}
