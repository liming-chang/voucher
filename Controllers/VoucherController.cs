using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Context;
using Test.Models;
using Newtonsoft.Json;
using System.Transactions;

namespace Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly TransLogContext _transLogcontext;//传输日志上下文
        private readonly TVoucherContext _tVoucherContext;//T+凭证上下文
        private readonly NCVoucherContext _ncVoucherContext;//NC凭证上下文
        public VoucherController(TransLogContext tlc,TVoucherContext tvc,NCVoucherContext nvc)
        {
            _transLogcontext = tlc;
            _tVoucherContext = tvc;
            _ncVoucherContext = nvc;

            if (_tVoucherContext.tVouchers.Count() == 0)
            {
                for(int i = 0; i < 3; i++)
                {
                    _tVoucherContext.tVouchers.AddRange(
                        new TVoucher {  ItemNum = $"00{i}" + 0, Count = 3, Price = 3.00, TVoucer = $"{i}" },
                        new TVoucher {  ItemNum = $"00{i}" + 1, Count = 3, Price = 2.00, TVoucer = $"{i}" },
                        new TVoucher {  ItemNum = $"00{i}" + 2, Count = 2, Price = 3.50, TVoucer = $"{i}" });
                    Console.WriteLine($"{i*3},{i*3+1},{i*3+2}");
                    _tVoucherContext.SaveChanges();
                }
            }
        }


        /*
         * 
         * 返回T+凭证
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TVoucher>>> GetTvc()
        {
            return await _tVoucherContext.tVouchers.ToListAsync();
        }

        /*
         * 
         * 返回NC凭证
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NCVoucher>>> GetNCvc()
        {
            return await _ncVoucherContext.ncVouchers.ToListAsync();
        }

        /*
         * 
         * 
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransLog>>> GetTransLog()
        {
            return await _transLogcontext.transLogs.ToListAsync();
        }

        /*
         *前端抓取T+凭证，将抓到的凭证信息存放到中间表中
         */
        [HttpGet]
        public async  Task<ActionResult<TransLog>> Fetch(int page,string username,int pageSize = 2)
        {
            List<TransLog> trans= new List<TransLog>();
            List<string> vouchers = await _tVoucherContext
                                            .tVouchers
                                            .Select(s=>s.TVoucer)
                                            .Distinct()
                                            .Skip((page-1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

            foreach (string voucher in vouchers)
            {

                //判定transLog表中是否已经加载该凭证，若未加载，加载到中间表中
                
                TransLog translog = _transLogcontext.transLogs
                                .Where(n => n.TVoucher == voucher)
                                .FirstOrDefault();
                if (translog == null)
                {
                    TransLog transLog = new TransLog
                    {
                        UserName = username,
                        TVoucher = voucher,
                        NCVoucher = voucher,
                        ExecTime = DateTime.Now,
                        Status = Status.unselected
                    };
                    _transLogcontext.transLogs.Add(transLog);
                    trans.Add(transLog);
                }
                else
                {
                    trans.Add(translog);
                }
                await _transLogcontext.SaveChangesAsync();
            }
            
            //layui数据表格需要的数据格式
            return Ok(new { 
                code=0,
                msg="",
                count=_transLogcontext.transLogs.Count(),
                data=trans });
                      
        }


        /*
         * 将T+凭证转化为NC凭证,转化前将中间表（transLog）中的凭证状态修改为正在写入(writing)
         * ,成功后，将凭证状态修改为已完成(finished),如果失败，修改为 failed
         */
        [HttpGet]
        public async Task<ActionResult> Send(string json)
        {
            //string json = Convert.ToString(obj.json);
            string[] vouchers = json.Split(",");

            //List<string> vouchers = new List<string>();
            //foreach (var translog in logs)
            //{
            //    vouchers.Add(translog.TVoucher);
            //}
            
            List<TVoucher> tvc;//接受前端发送的T+凭证
            foreach(string voucher in vouchers)
            {
                //从数据库中获取每个T+凭证所对应的信息
                tvc = await _tVoucherContext
                    .tVouchers
                    .Where(w => w.TVoucer == voucher)
                    .ToListAsync();

                //修改凭证状态为写入(writting)
                ModStatus(voucher, Status.writting);
               
                
                
                
                //using(var trans= new TransactionScope())
                //{

                //}
                
                foreach(TVoucher tv in tvc)
                {
                 
                    //将T+凭证发送到NC
                    _ncVoucherContext.ncVouchers.Add(
                         new NCVoucher
                         {
                             NCvoucher = tv.TVoucer,
                             ItemNum = tv.ItemNum,
                             Count = tv.Count,
                             Price = tv.Price
                         });
                }
                await _ncVoucherContext.SaveChangesAsync();

                //修改凭证状态为完成(finished)
                ModStatus(voucher, Status.finished);
            }

            return Ok(new {data = "success" });
        }


        /*
         * 在transLog表中查询与voucher对应的数据项，并将其状态修改为status
         * voucher 将要处理的凭证
         * statue  将要设定的凭证状态
         */

        public async void ModStatus(string  voucher,Status status)
        {
            var transLogs = await _transLogcontext
                            .transLogs
                            .Where(b => b.TVoucher == voucher)
                            .ToListAsync();
            foreach (TransLog trans in transLogs)
            {
                trans.Status = status;
                _transLogcontext.transLogs.Update(trans);
            }
            await _transLogcontext.SaveChangesAsync();
        }



    }
}
