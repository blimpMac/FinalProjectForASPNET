using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly AspnetfpDbContext _context;
    private readonly IMapper _mapper;

    public DashboardController(AspnetfpDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("visitors/monthly")]
    public async Task<ActionResult> GetMonthlyVisitors()
    {
        var now = DateTime.Now;
        var startOfMonth = new DateOnly(now.Year, now.Month, 1);
        var endOfMonth = DateOnly.FromDateTime(now);

        int count = await _context.BorrowerTbls
            .AsNoTracking()
            .Where(b => b.BorrowedOn >= startOfMonth && b.BorrowedOn <= endOfMonth)
            .Select(b => b.BorrowedOn)
            .Distinct()
            .CountAsync();

        return Ok(new { count });
    }

    [HttpGet("borrowers/active/count")]
    public async Task<ActionResult> GetActiveBorrowerCount()
    {
        int count = await _context.BorrowerTbls
            .AsNoTracking()
            .Where(b => !b.ReturneeTbls.Any())
            .CountAsync();

        return Ok(new { count });
    }

    [HttpGet("borrowers/today/count")]
    public async Task<ActionResult> GetBorrowedTodayCount()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        int count = await _context.BorrowerTbls
            .AsNoTracking()
            .Where(b => b.BorrowedOn == today)
            .CountAsync();

        return Ok(new { count });
    }

    [HttpGet("books/overdue/count")]
    public async Task<ActionResult> GetOverdueBooksCount()
    {
        var dueThreshold = DateOnly.FromDateTime(DateTime.Now.AddDays(-14));
        int count = await _context.BorrowerTbls
            .AsNoTracking()
            .Where(b => !b.ReturneeTbls.Any() && b.BorrowedOn < dueThreshold)
            .CountAsync();

        return Ok(new { count });
    }

    [HttpGet("borrowers/recent")]
    public async Task<ActionResult<IEnumerable<Borrower_ReadDTO>>> GetRecentBorrowers([FromQuery] int limit = 5)
    {
        var borrowers = await _context.BorrowerTbls
            .AsNoTracking()
            .Include(b => b.ReturneeTbls)
            .OrderByDescending(b => b.BorrowedOn)
            .ThenByDescending(b => b.Time)
            .Take(limit)
            .ToListAsync();

        var mapped = _mapper.Map<List<Borrower_ReadDTO>>(borrowers);
        return Ok(mapped);
    }

    [HttpGet("summary")]
    public async Task<ActionResult> GetLibrarySummaryForPieChart()
    {
        var now = DateTime.Now;
        var startOfMonth = new DateOnly(now.Year, now.Month, 1);
        var dueThreshold = DateOnly.FromDateTime(now.AddDays(-14));

        var activeTask = _context.BorrowerTbls.AsNoTracking().Where(b => !b.ReturneeTbls.Any()).CountAsync();
        var returnedTask = _context.ReturneeTbls.AsNoTracking().Where(r => r.ReturnedOn >= startOfMonth).CountAsync();
        var overdueTask = _context.BorrowerTbls.AsNoTracking().Where(b => !b.ReturneeTbls.Any() && b.BorrowedOn < dueThreshold).CountAsync();
        var totalBorrowsTask = _context.BorrowerTbls.AsNoTracking().CountAsync();
        var totalReturnsTask = _context.ReturneeTbls.AsNoTracking().CountAsync();

        await Task.WhenAll(activeTask, returnedTask, overdueTask, totalBorrowsTask, totalReturnsTask);

        int active = activeTask.Result;
        int returned = returnedTask.Result;
        int overdue = overdueTask.Result;
        int totalBorrows = totalBorrowsTask.Result;
        int totalReturns = totalReturnsTask.Result;

        int estimatedAvailable = Math.Max(0, totalBorrows - active - returned - overdue + totalReturns);

        return Ok(new
        {
            labels = new[] { "Active Borrows", "Returned This Month", "Overdue Books", "Estimated Available" },
            data = new[] { active, returned, overdue, estimatedAvailable }
        });
    }

    [HttpGet("trends")]
    public async Task<ActionResult> GetOptimizedTrends([FromQuery] int days = 30)
    {
        var now = DateTime.UtcNow;
        var startDate = DateOnly.FromDateTime(now.AddDays(-days));
        var endDate = DateOnly.FromDateTime(now);

        var borrowTrends = await _context.BorrowerTbls
            .AsNoTracking()
            .Where(b => b.BorrowedOn >= startDate && b.BorrowedOn <= endDate)
            .GroupBy(b => b.BorrowedOn)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.Count);

        var result = Enumerable.Range(0, (endDate.DayNumber - startDate.DayNumber) + 1)
            .Select(offset => new
            {
                Date = startDate.AddDays(offset),
                Count = borrowTrends.TryGetValue(startDate.AddDays(offset), out var c) ? c : 0
            })
            .ToList();

        return Ok(new
        {
            labels = result.Select(x => x.Date.ToString("MMM d")),
            datasets = new[]
            {
                new
                {
                    label = "Books Borrowed",
                    data = result.Select(x => x.Count),
                    borderWidth = 2,
                    tension = 0.3
                }
            }
        });
    }
}