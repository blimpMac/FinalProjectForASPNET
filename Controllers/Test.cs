using AutoMapper;
using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly AspnetfpDbContext _context;
        private readonly IMapper _mapper;

        public TestController(AspnetfpDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("books")]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.BookTbls.ToListAsync();
            var mapped = _mapper.Map<IEnumerable<BookDTO_ReadDTO>>(books);
            return Ok(mapped);
        }

        [HttpGet("borrowers")]
        public async Task<IActionResult> GetBorrowers()
        {
            var borrowers = await _context.BorrowerTbls.ToListAsync();
            var mapped = _mapper.Map<IEnumerable<Borrower_ReadDTO>>(borrowers);
            return Ok(mapped);
        }

        [HttpGet("returnees")]
        public async Task<IActionResult> GetReturnees()
        {
            var returnees = await _context.ReturneeTbls.ToListAsync();
            var mapped = _mapper.Map<IEnumerable<Returnee_ReadDTO>>(returnees);
            return Ok(mapped);
        }
        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<LibraryTransaction_ReadDTO>>> GetTransactions()
        {
            var transactions = await _context.LibraryTransactionVws.ToListAsync();
            var result = _mapper.Map<IEnumerable<LibraryTransaction_ReadDTO>>(transactions);
            return Ok(result);
        }
    }
}
