using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Users.Models;
using PetaPoco;


namespace BankSystem.Users.Servies
{
  public  class UserReg
    {
        Repositories.UserReg ru = new Repositories.UserReg();

        /// <summary>
        /// 新用户注册
        /// </summary>
        /// <param name="uInfo">用户实体类</param>
        /// <returns></returns>

        public object UsersReg(UserInfo uInfo)
        {
            //uInfo.LoginPass = ReUse.BllUtility.MD5AndSHA1.MD5Encode(uInfo.LoginPass, "32");
            //uInfo.TradePass = ReUse.BllUtility.MD5AndSHA1.MD5Encode(uInfo.TradePass, "32");
            return ru.UsersReg(uInfo);
        }

        /// <summary>
        /// 用户登录&管理员登陆
        /// </summary>
        /// <param name="uInfo">用户实体类</param>
        /// <returns></returns>
        public UserInfo Login(UserInfo uInfo)
        {
           // uInfo.LoginPass = ReUse.BllUtility.MD5AndSHA1.MD5Encode(uInfo.LoginPass, "32");
            return ru.Login(uInfo);
        }
        /// <summary>
        /// 获取主菜单信息
        /// </summary>
        /// <param name="Account">用户实体类</param>
        /// <returns></returns>
        public UserInfo GetUserDetail(int Account)
        {
            return ru.GetUserDetail(Account);
        }
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="uInfo">用户实体类</param>
        /// <returns></returns>
        public object UserUpdate(UserInfo uInfo)
        {
            return ru.UserUpdate(uInfo);
        }
       
    }
}
