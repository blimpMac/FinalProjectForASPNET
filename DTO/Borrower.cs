namespace FinalProject.DTOs
{
    public class Borrower_ReadDTO
    {
        public int BorrowID { get; set; }
        public string BookISBN { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public DateOnly BorrowedOn { get; set; }
        public TimeOnly Time { get; set; }
    }

    public class Borrower_CreateDTO
    {
        public string BookISBN { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
    }

    public class Borrower_UpdateDTO
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
    }
}