using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Borrower2Tbl
    {
        [Key]
        [Required]
        public string Isbnnumber { get; set; } = null!;
        [Required(ErrorMessage = "Book name is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Book name must be between 2 and 200 characters.")]
        public string BookName { get; set; } = null!;
        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Author name must be between 2 and 100 characters.")]
        public string AuthorName { get; set; } = null!;
        [Required(ErrorMessage = "Date published is required.")]
        public DateOnly DatePublished { get; set; }
        [Required(ErrorMessage = "Printed year is required.")]
        public int PrintedYear { get; set; }
        [Required(ErrorMessage = "Printed by is required.")]
        [StringLength(100, ErrorMessage = "Printed by cannot exceed 100 characters.")]
        public string PrintedBy { get; set; } = null!;
        [Required(ErrorMessage = "Availability is required.")]
        [StringLength(50, ErrorMessage = "Availability cannot exceed 50 characters.")]
        public string Availability { get; set; } = null!;
        public virtual ICollection<BorrowerTbl> BorrowerTbls { get; set; } = new List<BorrowerTbl>();
    }
}