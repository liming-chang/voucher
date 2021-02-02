using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class TokenLog
    {
        public int Id { get; set; }//userID
        public Guid Token { get; set; }
        public DateTime DateTime { get; set; }
    }
}
