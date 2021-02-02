using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Context
{
    //NC凭证上下文
    public class NCVoucherContext : DbContext
    {
        public NCVoucherContext(DbContextOptions<NCVoucherContext> options)
            : base(options)
        {

        }

        public DbSet<NCVoucher> ncVouchers { get; set; }
    }
}
