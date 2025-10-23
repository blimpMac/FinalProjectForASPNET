namespace FinalProject.DTOs
{
    public class ReturneeDTO
    {
        public int ReturnID { get; set; }
        public int BorrowID { get; set; }
        public DateOnly ReturnedOn { get; set; }
        public string Condition { get; set; }
    }
}
