using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;


namespace BankSystem.Core
{
    public class SqlHelper
    {
        #region ConnectionString
        /// <summary>
        /// 从webConfiguration中读取连接串（只读）
        /// </summary>
        public static readonly string ConnString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// SQL语句执行并返回结果
        /// </summary>
        /// <param name="ConnString">数据库连接串</param>
        /// <param name="cmdType">SqlCommandType(如text普通SQL语句，StoredProcedure存储过程)</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParams">所用参数组</param>
        /// <returns>执行受影响的行数</returns>
        public static int ExecuteNonQuery(string ConnString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            int reValue = 0;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                    conn.Open();
                    reValue = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            return reValue;
        }
        /// <summary>
        /// SQL语句执行并返回结果(重载)
        /// </summary>
        /// <param name="conn">SQLConnection连接</param>
        /// <param name="cmdType">SqlCommandType(如text普通SQL语句，StoredProcedure存储过程)</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParams">所用参数组</param>
        /// <returns>执行受影响的行数</returns>
        public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            int reValue = 0;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                reValue = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            return reValue;
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// SQL查询语句执行（返回SqlDataReader）
        /// </summary>
        /// <param name="cmdType">SqlCommandType(如text普通SQL语句，StoredProcedure存储过程)</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParams">所用参数组</param>
        /// <returns>执行后返回SQLDataReader</returns>
        public static SqlDataReader ExecuteReader(string ConnString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            SqlConnection conn = new SqlConnection(ConnString);
            try
            {
                SqlDataReader reReader;
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                    conn.Open();
                    reReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                }
                return reReader;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        /// <summary>
        /// SQL查询语句执行（返回SqlDataReader）(c)
        /// </summary>
        /// <param name="conn">SQLConnection连接</param>
        /// <param name="cmdType">SqlCommandType(如text普通SQL语句，StoredProcedure存储过程)</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParams">所用参数组</param>
        /// <returns>>执行后返回SQLDataReader</returns>
        public static SqlDataReader ExecuteReader(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            SqlDataReader reReader;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                reReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
            }
            return reReader;
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// SQL查询语句执行(返回第一行)
        /// </summary>
        /// <param name="conn">SQLConnection连接</param>
        /// <param name="cmdType">SqlCommandType(如text普通SQL语句，StoredProcedure存储过程)</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParams">所用参数组</param>
        /// <returns>返回查询结果的第一行</returns>
        public static object ExecuteScalar(string ConnString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            object reValue;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                    conn.Open();
                    reValue = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                }
            }
            return reValue;
        }
        /// <summary>
        /// SQL查询语句执行(返回第一行)
        /// </summary>
        /// <param name="conn">SQLConnection连接</param>
        /// <param name="cmdType">SqlCommandType(如text普通SQL语句，StoredProcedure存储过程)</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParams">所用参数组</param>
        /// <returns>返回查询结果的第一行</returns>
        public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            object reValue;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                reValue = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            return reValue;
        }
        #endregion

        #region ExecuteDataTable
        /// <summary>
        /// SQL查询语句执行（返回DataTable）
        /// </summary>
        /// <param name="conn">SQLConnection连接</param>
        /// <param name="cmdType">SqlCommandType(如text普通SQL语句，StoredProcedure存储过程)</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParams">所用参数组</param>
        /// <returns>执行后返回DataTable</returns>
        public static DataTable ExecuteDataTable(string ConnString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            DataTable reData = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        sda.Fill(reData);
                    }
                    cmd.Parameters.Clear();
                }
            }
            return reData;
        }
        /// <summary>
        /// SQL查询语句执行（返回DataTable）
        /// </summary>
        /// <param name="conn">SQLConnection连接</param>
        /// <param name="cmdType">SqlCommandType(如text普通SQL语句，StoredProcedure存储过程)</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParams">所用参数组</param>
        /// <returns>执行后返回DataTable</returns>
        public static DataTable ExecuteDataTable(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            DataTable reData = new DataTable();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    sda.Fill(reData);
                }
                cmd.Parameters.Clear();
            }
            return reData;
        }
        #endregion

        #region CloseConnection
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="conn">SQLConnection</param>
        public static void CloseConnection(SqlConnection conn)
        {
            if (conn.State != ConnectionState.Closed)
                conn.Close();
        }
        #endregion

        #region PrepareCommand
        /// <summary>
        /// 创建一个SQLCommand
        /// </summary>
        /// <param name="cmd">SQLCommand</param>
        /// <param name="cmdType">SqlCommandType(如text普通SQL语句，StoredProcedure存储过程)</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParams">所用参数组</param>
        private static void PrepareCommand(SqlCommand cmd, CommandType cmdType, string cmdText, SqlParameter[] cmdParams)
        {
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (cmdParams != null)
            {
                foreach (SqlParameter param in cmdParams)
                {
                    cmd.Parameters.Add(param);
                }
            }
        }
        #endregion
    }
}
