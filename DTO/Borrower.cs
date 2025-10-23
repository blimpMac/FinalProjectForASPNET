namespace FinalProject.DTOs
{
    public class BorrowerDTO
    {
        public int BorrowID { get; set; }
        public string BookISBN { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public DateOnly BorrowedOn { get; set; }
        public string Time { get; set; }
    }
}
