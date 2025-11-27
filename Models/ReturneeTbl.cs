using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class ReturneeTbl
    {
        [Key]
        public int ReturnId { get; set; }
        [Required(ErrorMessage = "Borrow ID is required.")]
        public int BorrowId { get; set; }
        [Required(ErrorMessage = "Book title is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Book title must be between 2 and 200 characters.")]
        public string BookTitle { get; set; } = null!;
        [Required(ErrorMessage = "Returned date is required.")]
        public DateOnly ReturnedOn { get; set; }
        [Required(ErrorMessage = "Condition is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Condition must be between 2 and 200 characters.")]
        public string Condition { get; set; } = null!;
        [Required]
        public virtual BorrowerTbl Borrow { get; set; } = null!;
    }
}