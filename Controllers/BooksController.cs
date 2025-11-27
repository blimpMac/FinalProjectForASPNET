using AutoMapper;
using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly AspnetfpDbContext _context;
    private readonly IMapper _mapper;

    public BooksController(AspnetfpDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDTO_ReadDTO>>> GetBooks()
    {
        var books = await _context.BookTbls.ToListAsync();
        return _mapper.Map<List<BookDTO_ReadDTO>>(books);
    }

    [HttpGet("{isbn}")]
    public async Task<ActionResult<BookDTO_ReadDTO>> GetBookByIsbn(string isbn)
    {
        var book = await _context.BookTbls.FindAsync(isbn);
        if (book == null) return NotFound();
        return _mapper.Map<BookDTO_ReadDTO>(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookDTO_ReadDTO>> PostBook(BookDTO_CreateDTO dto)
    {
        if (await _context.BookTbls.AnyAsync(b => b.Isbnnumber == dto.ISBNNumber))
            return Conflict($"Book with ISBN {dto.ISBNNumber} already exists.");

        var newBook = _mapper.Map<BookTbl>(dto);
        newBook.Availability = "Available";
        newBook.DatePublished = DateOnly.FromDateTime(DateTime.Now);
        newBook.PrintedYear = DateTime.Now.Year;
        newBook.PrintedBy = "Not Specified";

        _context.BookTbls.Add(newBook);
        await _context.SaveChangesAsync();

        var readDto = _mapper.Map<BookDTO_ReadDTO>(newBook);
        return CreatedAtAction(nameof(GetBookByIsbn), new { isbn = newBook.Isbnnumber }, readDto);
    }

    [HttpPut("{isbn}")]
    public async Task<IActionResult> PutBook(string isbn, BookDTO_UpdateDTO dto)
    {
        var book = await _context.BookTbls.FindAsync(isbn);
        if (book == null)
        {
            return NotFound("Book not found.");
        }

        _mapper.Map(dto, book);
        await _context.SaveChangesAsync();

        return NoContent(); 
    }
    [HttpDelete("{isbn}")]
    public async Task<IActionResult> DeleteBook(string isbn)
    {
        var book = await _context.BookTbls
            .Include(b => b.BorrowerTbls)
            .FirstOrDefaultAsync(b => b.Isbnnumber == isbn);

        if (book == null) return NotFound("Book not found.");

        var hasOutstandingLoan = book.BorrowerTbls.Any(bt => !bt.ReturneeTbls.Any());
        if (hasOutstandingLoan)
            return Conflict("Cannot delete book: it has an outstanding loan transaction.");

        _context.BookTbls.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}