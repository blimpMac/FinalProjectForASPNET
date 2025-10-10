using System.ComponentModel.DataAnnotations;

namespace Midterm_Project.DTO
{
    public class CookBookRecipe_ReqField
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 20 characters.")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Food name is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Food name must be between 2 and 30 characters.")]
        public string FoodName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Ingredients are required.")]
        [MinLength(5, ErrorMessage = "Ingredients must be at least 5 characters long.")]
        public string Ingredients { get; set; } = string.Empty;

        [Required(ErrorMessage = "Instructions are required.")]
        [MinLength(10, ErrorMessage = "Instructions must be at least 10 characters long.")]
        public string Instructions { get; set; } = string.Empty;

        [Required(ErrorMessage = "MealType are required.")]
        [StringLength(20, ErrorMessage = "Meal type cannot exceed 20 characters.")]
        public string? MealType { get; set; }
    }
}