using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    //传输日志上下文
    public class TransLogContext : DbContext
    {
        public TransLogContext(DbContextOptions<TransLogContext> options)
            : base(options)
        {

        }

        public DbSet<TransLog> transLogs { get; set; }
    }
}
