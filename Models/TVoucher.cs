using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class TVoucher
    {
        public int Id { get; set; }// 凭证ID   在数据库中的ID
        public string TVoucer { get; set; }     //凭证编号
        public string ItemNum { get; set; }     //商品编号
        public int Count { get; set; }          //商品数量
        public double Price { get; set; }       //商品价格

    }
}
