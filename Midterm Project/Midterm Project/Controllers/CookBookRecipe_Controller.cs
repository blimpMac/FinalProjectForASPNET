using AutoMapper;
using Midterm_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midterm_Project.DTO;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly CookBookRecipeDbContext _context;
    private readonly IMapper _mapper;

    public RecipesController(CookBookRecipeDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CookBook_RecipeReadDTO>>> GetAll()
    {
        var recipes = await _context.RecipesTB.ToListAsync();
        var result = _mapper.Map<IEnumerable<CookBook_RecipeReadDTO>>(recipes);
        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult<CookBook_RecipeReadDTO>> Create(CookBookRecipe_ReqField recipeDto)
    {
        var recipe = _mapper.Map<RecipesTb>(recipeDto);
        _context.RecipesTB.Add(recipe);
        await _context.SaveChangesAsync(); 
        var readDto = _mapper.Map<CookBook_RecipeReadDTO>(recipe);
        return Ok(readDto);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CookBook_RecipeUpdateDTO recipeDto)
    {
        var recipe = await _context.RecipesTB.FindAsync(id);
        if (recipe == null) return NotFound();

        _mapper.Map(recipeDto, recipe);
        if (!TryValidateModel(recipe)) return BadRequest(ModelState);

        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var recipe = await _context.RecipesTB.FindAsync(id);
        if (recipe == null) return NotFound();

        _context.RecipesTB.Remove(recipe);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpGet("username/{username}")]
    public async Task<ActionResult<IEnumerable<CookBook_RecipeReadDTO>>> GetByUserName(string username)
    {
        var recipes = await _context.RecipesTB
            .Where(r => r.Username.ToLower().Contains(username.ToLower()))
            .ToListAsync();

        if (!recipes.Any())
            return NotFound($"No Food made found for username: {username}");

        var result = _mapper.Map<IEnumerable<CookBook_RecipeReadDTO>>(recipes);
        return Ok(result);
    }

    [HttpGet("foodname/{foodName}")]
    public async Task<ActionResult<IEnumerable<CookBook_RecipeReadDTO>>> GetByFoodName(string foodName)
    {
        var recipes = await _context.RecipesTB
            .Where(r => r.FoodName.ToLower().Contains(foodName.ToLower()))
            .ToListAsync();

        if (!recipes.Any()) return NotFound($"There is No: {foodName} in the database.");

        var result = _mapper.Map<IEnumerable<CookBook_RecipeReadDTO>>(recipes);
        return Ok(result);
    }
    [HttpGet("mealtype/{mealType}")]
    public async Task<ActionResult<IEnumerable<CookBook_RecipeReadDTO>>> GetByMealType(string mealType)
    {
        var recipes = await _context.RecipesTB
            .Where(r => r.MealType != null && r.MealType.ToLower() == mealType.ToLower())
            .ToListAsync();

        if (!recipes.Any()) return NotFound($"No recipes found for meal type: {mealType}");

        var result = _mapper.Map<IEnumerable<CookBook_RecipeReadDTO>>(recipes);
        return Ok(result);
    }
}