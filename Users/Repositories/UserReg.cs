using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Users.Models;
using PetaPoco;
using System.Data.SqlClient;
using System.Data;
using BankSystem.Core;

namespace BankSystem.Users.Repositories
{
   public class UserReg
    {

        private Database DB = new Database("ConnString");
        /// <summary>
        /// 新用户注册
        /// </summary>
        /// <param name="uInfo">用户实体类</param>
        /// <returns></returns>
        public object UsersReg(UserInfo uInfo)
        {
            //PetaPoco进行用户注册，前提主键是递增项 并且是int类型
            return DB.Insert("Tb_CUSTOMER", "Account", uInfo);
        }


        /// <summary>
        /// 用户or管理员登陆
        /// </summary>
        /// <param name="uInfo">用户实体类</param>
        /// <returns></returns>
        public UserInfo Login(UserInfo uInfo)
        {
            //根据账号跟密码获取账户的信息
            UserInfo uinfo = new UserInfo();
            string sql = "select Account,LoginPass from Tb_CUSTOMER where Account=@Account and LoginPass = @LoginPass";
            SqlParameter[] sParams = new SqlParameter[]
            {
                 new SqlParameter("@Account",SqlDbType.Int),
                new SqlParameter("@LoginPass",SqlDbType.VarChar)
            };
            sParams[0].Value = uInfo.Account;
            sParams[1].Value = uInfo.LoginPass;
            using (SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnString, CommandType.Text, sql, sParams))
            {
                //数据库读取出来，进行字段赋值
                if (sdr.Read())
                {
                    uinfo.Account = Convert.ToInt32(sdr["Account"]);
                    uinfo.LoginPass = Convert.ToString(sdr["LoginPass"]);
                    
                }
            }

            return uinfo;
        }


        /// <summary>
        /// 主菜单信息
        /// </summary>
        /// <param name="Account">（主键）账号</param>
        /// <returns></returns>
        public UserInfo GetUserDetail(int Account)
        {
            //通过主键获取用户的信息
            Sql sql = Sql.Builder.Append("select * from Tb_CUSTOMER where Account = @0", Account);
            return DB.FirstOrDefault<UserInfo>(sql);
        }
      
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="uInfo">用户实体类</param>
        /// <returns></returns>
        public object UserUpdate(UserInfo uInfo)
        {
            //根据主键进行修改信息
            return DB.Update("Tb_CUSTOMER", "Account", uInfo);
        }
     
    }
}
