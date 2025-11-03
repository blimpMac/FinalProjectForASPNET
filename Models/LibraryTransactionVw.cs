using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class LibraryTransactionVw
    {
        [Required(ErrorMessage = "ISBN number is required.")]
        public string Isbnnumber { get; set; } = null!;
        [Required(ErrorMessage = "Book name is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Book name must be between 2 and 200 characters.")]
        public string BookName { get; set; } = null!;
        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Author name must be between 2 and 100 characters.")]
        public string AuthorName { get; set; } = null!;
        [Required]
        public int BorrowId { get; set; }
        [Required(ErrorMessage = "Borrower name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Borrower name must be between 2 and 100 characters.")]
        public string BorrowerName { get; set; } = null!;
        [Required(ErrorMessage = "Borrower year is required.")]
        [Range(1, 8, ErrorMessage = "Borrower year must be between 1 and 8.")]
        public int BorrowerYear { get; set; }
        [Required(ErrorMessage = "Course is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Course must be between 2 and 100 characters.")]
        public string Course { get; set; } = null!;
        [Required(ErrorMessage = "Section is required.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Section must be between 1 and 10 characters.")]
        public string Section { get; set; } = null!;

        [Required(ErrorMessage = "Borrowed date is required.")]
        public DateOnly BorrowedOn { get; set; }

        [Required(ErrorMessage = "Borrowed time is required.")]
        public TimeOnly BorrowedTime { get; set; }
        public int? ReturnId { get; set; }
        public DateOnly? ReturnedOn { get; set; }
        [StringLength(200, ErrorMessage = "Condition cannot exceed 200 characters.")]
        public string? Condition { get; set; }
        [Required(ErrorMessage = "Loan status is required.")]
        [StringLength(50, ErrorMessage = "Loan status cannot exceed 50 characters.")]
        public string LoanStatus { get; set; } = null!;
    }
}