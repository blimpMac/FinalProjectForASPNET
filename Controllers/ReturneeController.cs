using AutoMapper;
using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ReturneeTblsController : ControllerBase
{
    private readonly AspnetfpDbContext _context;
    private IMapper _mapper;
    public ReturneeTblsController(AspnetfpDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> GetReturns()
    {
        var returns = await _context.ReturneeTbls.ToListAsync();
        var mapped = _mapper.Map<IEnumerable<Returnee_ReadDTO>>(returns);
        return Ok(mapped);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReturn(int id)
    {
        var returnRecord = await _context.ReturneeTbls.FindAsync(id);
        if (returnRecord == null) return NotFound();
        var mapped = _mapper.Map<Returnee_ReadDTO>(returnRecord);
        return Ok(mapped);
    }
    [HttpPost]
    public async Task<ActionResult<ReturneeTbl>> PostReturn(ReturneeTbl newReturn)
    {
        var loanRecord = await _context.BorrowerTbls.Include(b => b.BookIsbnNavigation)
            .FirstOrDefaultAsync(b => b.BorrowId == newReturn.BorrowId);
        if (loanRecord == null) return NotFound("Loan record not found.");
        if (loanRecord.BookIsbnNavigation.Availability == "Available") return Conflict("This book is already marked as available (has been returned).");
        loanRecord.BookIsbnNavigation.Availability = "Available";
        _context.Entry(loanRecord.BookIsbnNavigation).State = EntityState.Modified;
        _context.ReturneeTbls.Add(newReturn);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetReturn), new { id = newReturn.ReturnId }, newReturn);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutReturn(int id, ReturneeTbl returnUpdate)
    {
        if (id != returnUpdate.ReturnId) return BadRequest("ReturnId mismatch.");
        var existingReturn = await _context.ReturneeTbls.AsNoTracking().FirstOrDefaultAsync(r => r.ReturnId == id);
        if (existingReturn == null) return NotFound();
        if (existingReturn.BorrowId != returnUpdate.BorrowId) return BadRequest("Cannot change BorrowId on an existing return record.");
        _context.Entry(returnUpdate).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.ReturneeTbls.Any(e => e.ReturnId == id)) return NotFound();
            throw;
        }
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReturn(int id)
    {
        var returnRecord = await _context.ReturneeTbls.FindAsync(id);
        if (returnRecord == null) return NotFound("Return record not found.");

        var borrowerRecord = await _context.BorrowerTbls.Include(b => b.BookIsbnNavigation)
            .FirstOrDefaultAsync(b => b.BorrowId == returnRecord.BorrowId);
        if (borrowerRecord != null)
        {
            borrowerRecord.BookIsbnNavigation.Availability = "Borrowed";
            _context.Entry(borrowerRecord.BookIsbnNavigation).State = EntityState.Modified;
        }
        _context.ReturneeTbls.Remove(returnRecord);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}