using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenLogController : ControllerBase
    {
        private readonly TokenLogContext _context;
        public TokenLogController(TokenLogContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TokenLog>>> getTokens()
        {
            return await _context.tokenLogs.ToListAsync();
        }
    }
}
