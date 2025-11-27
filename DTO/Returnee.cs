namespace FinalProject.DTOs
{
    public class Returnee_ReadDTO
    {
        public int ReturnID { get; set; }
        public int BorrowID { get; set; }
        public string BookTitle { get; set; } = null!;
        public DateOnly ReturnedOn { get; set; }
        public string Condition { get; set; }
    }
    public class Returnee_CreateDTO
{
    public int ReturnID { get; set; }
    public int BorrowID { get; set; }
    public string BookTitle { get; set; } = null!;
    public DateOnly ReturnedOn { get; set; }
    public string Condition { get; set; }
}
    public class Returnee_UpdateDTO
{
    public int ReturnID { get; set; }
    public int BorrowID { get; set; }
    public string BookTitle { get; set; } = null!;
    public DateOnly ReturnedOn { get; set; }
    public string Condition { get; set; }
    }
}