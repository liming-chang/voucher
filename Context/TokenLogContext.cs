using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class TokenLogContext :DbContext
    {
        

        public TokenLogContext(DbContextOptions<TokenLogContext> options)
            :base(options)
        {

        }

        //protected override void onconfiguring(dbcontextoptionsbuilder optionsbuilder)
        //{
        //    base.onconfiguring(optionsbuilder);
        //    optionsbuilder.useinmemorydatabase("test");
        //}
        public DbSet<TokenLog> tokenLogs { get; set; }
    }
}
