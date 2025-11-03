using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class BorrowerTbl
    {
        [Key]
        public int BorrowId { get; set; }
        [Required(ErrorMessage = "Book ISBN is required.")]
        public string BookIsbn { get; set; } = null!;
        [Required(ErrorMessage = "Borrower name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Year is required.")]
        [Range(1, 8, ErrorMessage = "Year must be between 1 and 8.")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Course is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Course must be between 2 and 100 characters.")]
        public string Course { get; set; } = null!;
        [Required(ErrorMessage = "Section is required.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Section must be between 1 and 10 characters.")]
        public string Section { get; set; } = null!;
        [Required(ErrorMessage = "Borrowed date is required.")]
        public DateOnly BorrowedOn { get; set; }
        [Required(ErrorMessage = "Time borrowed is required.")]
        public TimeOnly Time { get; set; }
        [Required]
        public virtual BookTbl BookIsbnNavigation { get; set; } = null!;
        public virtual ICollection<ReturneeTbl> ReturneeTbls { get; set; } = new List<ReturneeTbl>();
    }
}