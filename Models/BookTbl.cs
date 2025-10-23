namespace FinalProject.Models;
public  class BookTbl
{
    public string Isbnnumber { get; set; } = null!;
    public string BookName { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public DateOnly DatePublished { get; set; }
    public int PrintedYear { get; set; }
    public string PrintedBy { get; set; } = null!;
    public string Availability { get; set; } = null!;
    public virtual ICollection<BorrowerTbl> BorrowerTbls { get; set; } = new List<BorrowerTbl>();
}