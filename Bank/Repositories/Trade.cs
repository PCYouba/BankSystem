using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Users.Models;
using BankSystem.Core;
using BankSystem.Bank.Models;
using PetaPoco;

namespace BankSystem.Bank.Repositories
{
  public  class Trade
    {
        private Database DB = new Database("ConnString");
        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="Account">主键（账号）</param>
        /// <returns></returns>
        public int Inmoney(int Account, decimal balance)
        {           
            //根据主键Account 从界面获取的金额进行数据库操作
            UserInfo uInfo = new UserInfo();
            //int i;
            //修改账户的余额信息
            string sql = "update Tb_CUSTOMER set Balance = Balance + @Balance where Account=@Account";
            SqlParameter[] sParams = new SqlParameter[]
            {
                 new SqlParameter("@Balance", SqlDbType.Decimal),
                 new SqlParameter("@Account", SqlDbType.Int)
            };
            sParams[0].Value = balance;//进行赋值
            sParams[1].Value = Account;
            return SqlHelper.ExecuteNonQuery(SqlHelper.ConnString, CommandType.Text, sql, sParams);
        }
        /// <summary>
        /// 记录存款信息
        /// </summary>
        /// <param name="uInfo">用户实体类</param>
        /// <returns></returns>
        public object AdminInmoney(TradeInfo tInfo)
        {
            //用PetaPoco进行记录存款信息操作
            return DB.Insert("Tb_Trade", "TradeCode", tInfo);
        }
        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="Account">主键（账号）</param>
        /// <param name="balance">余额</param>
        /// <returns></returns>
        public int Outmoney(int Account, decimal balance)
        {
            //跟存款操作相反
            UserInfo uInfo = new UserInfo();
            string sql = "update Tb_CUSTOMER set Balance = Balance - @Balance where Account=@Account";
            SqlParameter[] sParams = new SqlParameter[]
            {
                 new SqlParameter("@Balance", SqlDbType.Decimal),
                 new SqlParameter("@Account", SqlDbType.Int)
            };
            sParams[0].Value = balance;//赋值
            sParams[1].Value = Account;
            return SqlHelper.ExecuteNonQuery(SqlHelper.ConnString, CommandType.Text, sql, sParams);
        }
        /// <summary>
        /// 取出之后存入用户
        /// </summary>
        /// <returns></returns>

        public UserInfo TransInmoney(UserInfo uInfo)
        {           
            //从该用户取出相应的金额存入目标账户代码
            string sql = "update Tb_CUSTOMER set Balance = Balance + @Balance where Account=@Account";
            SqlParameter[] sParams = new SqlParameter[]
            {
                 new SqlParameter("@Balance", SqlDbType.Decimal),
                 new SqlParameter("@Account", SqlDbType.Int)
            };
            sParams[0].Value = uInfo.Balance;
            sParams[1].Value = uInfo.Account;
            using (SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnString, CommandType.Text, sql, sParams))
            {
                //如果数据库进行了读取
                if (sdr.Read())
                {
                    //为两个字段附上数据库的对应的值
                    uInfo.Account = Convert.ToInt32(sdr["UserID"]);
                    uInfo.Balance = Convert.ToDecimal(sdr["Balance"]);
                }
            }

            return uInfo;
        }
        /// <summary>
        /// 注销当前账户
        /// </summary>
        /// <param name="Account">账号（主键）</param>
        /// <returns></returns>
        public int DeleteUser(int Account)
        {
            //改变用户的状态（2表示注销当前的用户）
            Sql sql = Sql.Builder.Append("update Tb_CUSTOMER set Status = 2 where Account=@0", Account);
            return DB.Execute(sql);//除了select语句都可以用它执行
        }

        /// <summary>
        /// 交易查询（分页显示当前用户的交易信息）
        /// </summary>
        /// <param name="ItemsPerPage">一页几条数据</param>
        /// <returns></returns>

        public Page<TradeInfo> GetTradeList(long page, long ItemsPerPage, int Account)
        {
            //获取全部的信息 根据主键
            string where = string.Format("Account = {0}", Account);
            Sql sql = Sql.Builder
                .Select("*")
                .From("Tb_Trade")
                .Where(where);
            return DB.Page<TradeInfo>(page, ItemsPerPage, sql);
        }


        /// <summary>
        /// 交易查询（根据时间显示当前用户的交易记录）
        /// </summary>
        /// <param name="ItemsPerPage">一页几条数据</param>
        /// <returns></returns>

        public Page<TradeInfo> GetTradeLists(long page, long ItemsPerPage, int Account, DateTime StartTime, DateTime FinishTime)
        {
            //根据所选择的时间段进行数据库查询，满足所需要的条件
            string where = string.Format("Account={0} and CreateDate>'{1}' and CreateDate<'{2}' ", Account, StartTime, FinishTime);
            Sql sql = Sql.Builder
                .Select("*")
                .From("Tb_Trade")
                .Where(where);

            return DB.Page<TradeInfo>(page, ItemsPerPage, sql);
        }

        /// <summary>
        /// 管理员冻结/解除冻结账户
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="ItemsPerPage">一页几条数据</param>
        /// <returns></returns>
        public Page<UserInfo> AdminSet(long page, long ItemsPerPage)
        {
            //用简单的方法获取所有的账户信息
            Sql sql = Sql.Builder
                .Select("*")
                .From("Tb_CUSTOMER")
                .Where("Account>=1");
            // .OrderBy("UserID desc");//降序
            // .OrderBy("UserID asc");//升序
            return DB.Page<UserInfo>(page, ItemsPerPage, sql);
        }
        /// <summary>
        /// 获取全部信息
        /// </summary>
        /// <returns>Account（主键）账号</returns>
        public UserInfo GetAll(int Account)
        {
            UserInfo uinfo = new UserInfo();
            string sql = "select * from Tb_CUSTOMER where Account=@Account ";
            SqlParameter[] sParams = new SqlParameter[]
            {
                 new SqlParameter("@Account",SqlDbType.Int)

            };
            sParams[0].Value = Account;
            using (SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnString, CommandType.Text, sql, sParams))
            {
                //数据库读取出来，字段赋值
                if (sdr.Read())
                {
                    uinfo.Account = Convert.ToInt32(sdr["Account"]);
                    uinfo.Sex = Convert.ToInt32(sdr["Status"]);
                    uinfo.Status = Convert.ToInt32(sdr["Status"]);
                }
            }

            return uinfo;
        }
        /// <summary>
        /// 默认显示所有账户的总余额
        /// </summary>
        /// <param name="Account">主键）账号</param>
        /// <returns></returns>
        public int SumMoney()
        {
            //获取余额的总和
            Sql sql = Sql.Builder.Append("select sum(Balance) from Tb_CUSTOMER where Account > 0");
            return DB.Execute(sql);//除了select语句都可以用它执行
        }

        /// <summary>
        /// 修改用户状态(冻结)
        /// </summary>
        /// <param name="Account">主键）账号</param>
        /// <returns></returns>
        public int UpdateStatus(int Account)
        {
            //修改用户状态（0代表用户冻结）
            Sql sql = Sql.Builder.Append("update Tb_CUSTOMER set Status = 0 where Account=@0", Account);
            return DB.Execute(sql);//除了select语句都可以用它执行
        }

        /// <summary>
        /// 修改用户状态(解除冻结)
        /// </summary>
        /// <param name="Account">（主键）账号</param>
        /// <returns></returns>
        public int UpdateStatusfin(int Account)
        {//修改用户状态（1代表用户状态正常）
            Sql sql = Sql.Builder.Append("update Tb_CUSTOMER set Status = 1 where Account=@0", Account);
            return DB.Execute(sql);//除了select语句都可以用它执行
        }
    }
}
