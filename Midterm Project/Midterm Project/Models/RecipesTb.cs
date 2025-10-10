using System.ComponentModel.DataAnnotations.Schema;

namespace Midterm_Project.Models;
public partial class RecipesTb
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string FoodName { get; set; } = null!;
    public string? Description { get; set; }
    public string Ingredients { get; set; } = null!;
    public string Instructions { get; set; } = null!;
    public string? MealType { get; set; }
}