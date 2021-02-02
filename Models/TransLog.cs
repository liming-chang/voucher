using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public enum Status
    {
        failed  =-1,       //失败
        writting =1,     //正在写入
        finished =2,      //已完成
        unselected =0    //未选择
    }
    public class TransLog
    {
        public int Id { get; set; }
        public string TVoucher { get; set; }    //T+凭证编号
        public DateTime ExecTime { get; set; }  //操作时间
        public string UserName { get; set; }       //操作人员
        public Status Status { get; set; }      //执行状态
        public string NCVoucher { get; set; }   //NC凭证编号
        public string VoucherMsge { get; set; } //凭证信息
        public string ErrorMsg { get; set; }    //错误信息


    }
}
