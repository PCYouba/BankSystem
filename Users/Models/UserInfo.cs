using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BankSystem.Users.Models
{
    public class UserInfo
    {
        /// <summary>
        /// 账号
        /// </summary>
        public int Account { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [RegularExpression(@"^\d{6}$", ErrorMessage = "密码必须为6位！")]
        public string LoginPass { get; set; }
        /// <summary>
        /// 交易密码
        /// </summary>
        [RegularExpression(@"^\d{6}$", ErrorMessage = "密码必须为6位！")]
        public string TradePass { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>            
        //[RegularExpression(@"^\d{15}$", ErrorMessage = "必须为15位或18位!")]
        public string IDCard { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }


    }
}