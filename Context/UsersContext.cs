using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class UsersContext :DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
        }

        public DbSet<Users> users { get; set; }
    }
}
