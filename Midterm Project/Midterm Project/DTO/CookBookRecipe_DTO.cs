namespace Midterm_Project.DTO
{
    public class CookBook_RecipeReadDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FoodName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Ingredients { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string? MealType { get; set; }
    }
    public class CookBook_RecipeCreateDTO
    {
        public string Username { get; set; } = string.Empty;
        public string FoodName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Ingredients { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string? MealType { get; set; }
    }
    public class CookBook_RecipeUpdateDTO
    {
        public string Username { get; set; } = string.Empty;
        public string FoodName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Ingredients { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string? MealType { get; set; }
    }
}