using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public class LibraryTransactionVw
{
    public string Isbnnumber { get; set; } = null!;
    public string BookName { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public int BorrowId { get; set; }
    public string BorrowerName { get; set; } = null!;
    public int BorrowerYear { get; set; }
    public string Course { get; set; } = null!;
    public string Section { get; set; } = null!;
    public DateOnly BorrowedOn { get; set; }
    public TimeOnly BorrowedTime { get; set; }
    public int? ReturnId { get; set; }
    public DateOnly? ReturnedOn { get; set; }
    public string? Condition { get; set; }
    public string LoanStatus { get; set; } = null!;
}
