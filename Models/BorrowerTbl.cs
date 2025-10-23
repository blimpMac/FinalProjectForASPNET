namespace FinalProject.Models;
public class BorrowerTbl
{
    public int BorrowId { get; set; }
    public string BookIsbn { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Year { get; set; }
    public string Course { get; set; } = null!;
    public string Section { get; set; } = null!;
    public DateOnly BorrowedOn { get; set; }
    public TimeOnly Time { get; set; }
    public virtual BookTbl BookIsbnNavigation { get; set; } = null!;
    public virtual ICollection<ReturneeTbl> ReturneeTbls { get; set; } = new List<ReturneeTbl>();
}
