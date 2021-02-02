using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class NCVoucher
    {
        public int Id { get; set; }
        public string NCvoucher { get; set; }//NC凭证编号
        public string ItemNum { get; set; } //商品编号
        public int Count { get; set; }//商品数量
        public double Price { get; set; }//商品价格
    }
}
