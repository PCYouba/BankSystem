using BankSystem.Bank.Models;
using BankSystem.Users.Models;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Bank.Servies
{
  public  class Trade
    {
        Repositories.Trade ru = new Repositories.Trade();
        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public int Inmoney(int Account, decimal balance)
        {
            return ru.Inmoney(Account, balance);
        }
        /// <summary>
        /// 记录存款信息
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="balance"></param>
        /// <returns></returns>

        public object AdminInmoney(TradeInfo tInfo)
        {
            return ru.AdminInmoney(tInfo);
        }
        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public int Outmoney(int Account, decimal balance)
        {
            return ru.Outmoney(Account, balance);
        }
        /// <summary>
        /// 当前用户转出金额
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public int TransOutmoney(int Account, decimal balance)
        {
            return ru.Outmoney(Account, balance);
        }
        /// <summary>
        /// 转账账户接收金额
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public UserInfo TransInmoney(UserInfo uInfo)
        {
            return ru.TransInmoney(uInfo);
        }
        /// <summary>
        /// 注销当前用户
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public int DeleteUser(int Account)
        {
            return ru.DeleteUser(Account);
        }
      
        /// <summary>
        /// 交易查询（默认显示当前用户的交易信息）
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
      
        public Page<TradeInfo> GetTradeList(long page, long ItemsPerPage, int Account)
        {
            return ru.GetTradeList(page, ItemsPerPage, Account);
        }


        /// <summary>
        ///  交易查询（根据时间显示当前用户的交易信息）
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="FinishTime"></param>
        /// <returns></returns>
       

        public Page<TradeInfo> GetTradeLists(long page, long ItemsPerPage, int Account, DateTime StartTime, DateTime FinishTime)
        {
            return ru.GetTradeLists(page, ItemsPerPage, Account, StartTime, FinishTime);
        }

        /// <summary>
        /// 管理员冻结/解除冻结账户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ItemsPerPage"></param>
        /// <returns></returns>
        public Page<UserInfo> AdminSet(long page, long ItemsPerPage)
        {
            return ru.AdminSet(page, ItemsPerPage);
        }
        /// <summary>
        /// 获取全部用户信息
        /// </summary>
        /// <returns></returns>
        public UserInfo GetAll(int Account)
        {
            return ru.GetAll(Account);
        }

        /// <summary>
        /// 默认显示所有账户的总余额
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public int SumMoney()
        {

            return ru.SumMoney();//除了select语句都可以用它执行
        }
        /// <summary>
        /// 修改用户状态（冻结）
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public int UpdateStatus(int Account)
        {
            return ru.UpdateStatus(Account);
        }

        /// <summary>
        /// 修改用户状态（解除冻结）
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public int UpdateStatusfin(int Account)
        {
            return ru.UpdateStatusfin(Account);
        }
    }
}
