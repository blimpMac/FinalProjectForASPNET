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
            var mapped = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(mapped);
        }

        [HttpGet("borrowers")]
        public async Task<IActionResult> GetBorrowers()
        {
            var borrowers = await _context.BorrowerTbls.ToListAsync();
            var mapped = _mapper.Map<IEnumerable<BorrowerDTO>>(borrowers);
            return Ok(mapped);
        }

        [HttpGet("returnees")]
        public async Task<IActionResult> GetReturnees()
        {
            var returnees = await _context.ReturneeTbls.ToListAsync();
            var mapped = _mapper.Map<IEnumerable<ReturneeDTO>>(returnees);
            return Ok(mapped);
        }
        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<LibraryTransactionDTO>>> GetTransactions()
        {
            var transactions = await _context.LibraryTransactionVws.ToListAsync();
            var result = _mapper.Map<IEnumerable<LibraryTransactionDTO>>(transactions);
            return Ok(result);
        }
    }
}
