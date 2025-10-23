namespace FinalProject.Models;
public class ReturneeTbl
{
    public int ReturnId { get; set; }
    public int BorrowId { get; set; }
    public DateOnly ReturnedOn { get; set; }
    public string Condition { get; set; } = null!;
    public virtual BorrowerTbl Borrow { get; set; } = null!;
}
