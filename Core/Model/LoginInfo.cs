using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Core.Model
{
   public class LoginInfo
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string  Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string pass { get; set; }
    }
}
