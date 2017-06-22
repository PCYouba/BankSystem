using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Bank.Models
{
  public class TradeInfo
    {
        /// <summary>
        /// 交易号
        /// </summary>
        public int TradeCode { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public int Account { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public int TradeType { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal MoneyNum { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime CreateDate { get; set; }

    }
}
