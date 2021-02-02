using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Context
{
    //T+凭证上下文
    public class TVoucherContext :DbContext
    {
        public TVoucherContext(DbContextOptions<TVoucherContext> options)
            : base(options)
        {

        }

        public DbSet<TVoucher> tVouchers { get; set; }
    }
}
