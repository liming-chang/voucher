using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UsersContext _context;
        private readonly TokenLogContext _tokenLogContext;
        
        public UserController(UsersContext context,TokenLogContext tokenLogContext)
        {
            _context = context;
            _tokenLogContext = tokenLogContext;
            if (_context.users.Count() == 0)
            {
                _context.users.Add(new Users { UserName = "admin", PassWord = "123456" });
                _context.SaveChanges();
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUser()
        {
            return await _context.users.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            return await _context.users.FindAsync(id);
        }


        //Post:api/user
        [HttpPost]
        public async Task<ActionResult> Add(Users users)
        {
            _context.users.Add(users);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = users.Id }, users);
        }

        //update 
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id,Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new {data="update success" });
        }

        //delete user
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
                return NotFound();
            _context.users.Remove(user);

            await _context.SaveChangesAsync();
            return NoContent();

        }


        //login system
        [HttpPost]
        public async Task<IActionResult> Login(Users user)
        {
            Users user1;
            
               List<Users> users=  await _context.users
                .Where(param => param.UserName == user.UserName)
                .ToListAsync();

            if (users.Count == 0)
                return Ok(new { data="username not exist!" });
            else
            {
                user1 = users[0];
                if (user1.PassWord == user.PassWord)
                {

                   if( await _tokenLogContext.tokenLogs.FindAsync(user.Id)!=null)
                    {
                        _tokenLogContext.Entry(new { Id = user.Id,Token = System.Guid.NewGuid(),DateTime = System.DateTime.Now}).State = EntityState.Modified;
                        await _tokenLogContext.SaveChangesAsync();
                    }else { 

                    _tokenLogContext.tokenLogs.Add(new TokenLog { Id = user.Id,Token= System.Guid.NewGuid(),DateTime =  System.DateTime.Now });
                    await _tokenLogContext.SaveChangesAsync();
                    }
                    return Ok(new { data = "success" });
                }
                else
                    return Ok(new { data= "password error" });
                   
       
            }
                
        }
    }
}
