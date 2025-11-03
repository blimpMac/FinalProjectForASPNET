using AutoMapper;
using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BorrowersController : ControllerBase
{
    private readonly AspnetfpDbContext _context;
    private readonly IMapper _mapper;

    public BorrowersController(AspnetfpDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Borrower_ReadDTO>>> GetBorrowers()
    {
        var borrowers = await _context.BorrowerTbls.ToListAsync();
        return Ok(_mapper.Map<List<Borrower_ReadDTO>>(borrowers));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Borrower_ReadDTO>> GetBorrower(int id)
    {
        var borrower = await _context.BorrowerTbls.FindAsync(id);
        if (borrower == null)
        {
            return NotFound(new { message = $"Borrow record {id} not found." });
        }
        return Ok(_mapper.Map<Borrower_ReadDTO>(borrower));
    }

    [HttpGet("returnees")]
    public async Task<IActionResult> GetReturnees()
    {
        var returnees = await _context.ReturneeTbls.ToListAsync();
        var mapped = _mapper.Map<IEnumerable<Returnee_ReadDTO>>(returnees);
        return Ok(mapped);
    }

    [HttpPost]
    public async Task<ActionResult<Borrower_ReadDTO>> PostBorrower(Borrower_CreateDTO dto)
    {
        var book = await _context.BookTbls.FindAsync(dto.BookISBN);
        if (book == null)
        {
            return NotFound(new { message = "Book not found for this ISBN." });
        }
        if (book.Availability == "Borrowed")
        {
            return Conflict(new { message = "Book is already currently loaned out." });
        }

        var newBorrower = _mapper.Map<BorrowerTbl>(dto);

        newBorrower.BorrowedOn = DateOnly.FromDateTime(DateTime.Now);
        newBorrower.Time = TimeOnly.FromDateTime(DateTime.Now);

        book.Availability = "Borrowed";

        _context.BorrowerTbls.Add(newBorrower);
        await _context.SaveChangesAsync();

        var readDto = _mapper.Map<Borrower_ReadDTO>(newBorrower);
        return CreatedAtAction(nameof(GetBorrower), new { id = newBorrower.BorrowId }, readDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBorrower(int id, Borrower_UpdateDTO dto)
    {
        var existingLoan = await _context.BorrowerTbls.FindAsync(id);
        if (existingLoan == null)
        {
            return NotFound(new { message = "Loan record not found." });
        }

        _mapper.Map(dto, existingLoan);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.BorrowerTbls.Any(e => e.BorrowId == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBorrower(int id)
    {
        var loan = await _context.BorrowerTbls
            .Include(l => l.ReturneeTbls)
            .FirstOrDefaultAsync(l => l.BorrowId == id);

        if (loan == null)
        {
            return NotFound(new { message = "Loan record not found." });
        }

        if (loan.ReturneeTbls.Any())
        {
            return Conflict(new { message = "Cannot delete: a return transaction already exists." });
        }

        var book = await _context.BookTbls.FindAsync(loan.BookIsbn);
        if (book != null)
        {
            book.Availability = "Available";
        }

        _context.BorrowerTbls.Remove(loan);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}