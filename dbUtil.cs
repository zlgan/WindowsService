using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace WindowsService1
{
    /// <summary>
    /// 数据操作工具类
    /// 作者：老郑
    /// 日期：2016年7月25日
    /// 版本：v1.1
    /// </summary>
    public class DBUtil
    {

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns>返回一个新的数据库连接</returns>
        public static SqlConnection GetSqlConnection()
        {
            string source = System.Configuration.ConfigurationManager.ConnectionStrings["myConn"].ConnectionString;
            return new SqlConnection(source);
        }

        /// <summary>
        /// 执行SQL语句 - 为了保证数据的完整性开启了事务
        /// </summary>
        /// <param name="sql">SQL语句（多条SQL语句可以用分号隔开）</param>
        /// <returns>返回受影响的行数</returns>
        public static int SqlExecute(string sql)
        {
            try
            {
                using (SqlConnection conn = GetSqlConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
            }
            catch (SqlException e)
            {
                throw;
            }
        }

        /// <summary>
        /// DataReader 方式读取数据
        /// </summary>
        /// <param name="sql">执行的SQL</param>
        public static void Sql2DataReader(string sql)
        {
            try
            {
                using (SqlConnection conn = GetSqlConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select top 10 * from Base_Area", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // TODO 自己的代码
                    }
                }
            }
            catch (SqlException e)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询出DataSet
        /// </summary>
        /// <param name="sql">查询的SQL语句</param>
        /// <returns></returns>
        public static DataSet Sql2DataSet(string sql)
        {
            try
            {
                SqlDataAdapter da = null;
                DataSet ds = new DataSet();
                using (SqlConnection conn = GetSqlConnection())
                {
                    conn.Open();
                    da = new SqlDataAdapter(sql, conn);
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (SqlException e)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询并返回DataTable
        /// </summary>
        /// <param name="sql">查询的SQL语句</param>
        /// <returns></returns>
        public static DataTable Sql2DataTable(string sql)
        {
            return Sql2DataSet(sql).Tables[0];
        }
    }
}