using Microsoft.EntityFrameworkCore;

namespace Midterm_Project.Models;

public partial class CookBookRecipeDbContext : DbContext
{
    public CookBookRecipeDbContext()
    {
    }

    public CookBookRecipeDbContext(DbContextOptions<CookBookRecipeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RecipesTb> RecipesTB { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=CookBook_RecipeDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipesTb>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RecipesT__3214EC073FE93237");

            entity.ToTable("RecipesTB");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.FoodName)
                .HasMaxLength(200)
                .HasColumnName("FoodName");
            entity.Property(e => e.MealType)
                .HasMaxLength(100)
                .HasDefaultValue("Breakfast");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
