using FinalProject.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AspnetfpDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("aspnetDBConnection")));
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/", () => "API is running.");

app.MapControllers();

app.Run();
