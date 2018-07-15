using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data.Common;


namespace Asky
{
    public class Demo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class Db
    {
        public static string dbtype = "tidb"; //用于切换数据库类型

        public static string mysql  = "server=127.0.0.1;port=3308;uid=root;pwd=demo!@#;database=test;SslMode=None;Character Set=utf8;useAffectedRows=false;ConnectionReset=false;Pooling=true;maxpoolsize=10000;minpoolsize=0;";
        public static string tidb = "";
        public static string postgresql = "";
        public static string sqlserver = "";

        #region 分页sql 兼容TiDB、MySql、MariaDB、PostgreSql、SqlServer
        /// <summary>
        /// 返回值分页Sql有一个起始空格，
        /// 分页Sql方便切换数据库TiDB、MySql、MariaDB、PostgreSql、SqlServer，
        /// SQL Server 2012及以上版本，新的分页语句OFFSET  FETCH，必须有order by，
        /// 参数默认值int pageIndex=1, int pageSize=1表示取出第一条数据，相当于top1语句，
        /// SqlPage()只传递int参数，没有sql注入风险，pageIndex从第1页开始，不是从第0页开始
        /// </summary>
        public static string SqlPage(int pageIndex = 1, int pageSize = 1)
        {
            if (dbtype == "sqlserver")
                return $" offset {(pageIndex - 1) * pageSize} rows fetch next {pageSize} rows only;";

            return $" limit {pageSize} offset {(pageIndex - 1) * pageSize}";
        } 
        #endregion

        #region TiDB、MySql、MariaDB
        /// <summary>
        /// TiDB、MySql、MariaDB一样
        /// </summary>
        public static MySqlConnection ConnMySql()
        {
            var conn = new MySqlConnection(mysql);
            if (conn.State != ConnectionState.Open)
                conn.Open();
            return conn;
        }

        /// <summary>
        /// 异步 TiDB、MySql、MariaDB一样
        /// </summary>
        public static async Task<MySqlConnection> ConnMySqlAsync()
        {
            var conn = new MySqlConnection(mysql); 
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();
            return conn;
        }
        #endregion

        #region PostgreSQL
        //public static Npgsql.NpgsqlConnection ConnPostgreSQL()
        //{
        //    var conn = new Npgsql.NpgsqlConnection(Config.PostgreSqlConn);
        //    if (conn.State != ConnectionState.Open)
        //        conn.Open();
        //    return conn;
        //}
        #endregion

        #region Sql Server
        public static SqlConnection ConnSqlServer()
        {
            var conn = new SqlConnection(sqlserver);
            if (conn.State != ConnectionState.Open)
                conn.Open();
            return conn;
        }

        public static async Task<SqlConnection> ConnSqlServerAsync()
        {
            var conn = new SqlConnection(sqlserver);
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();
            return conn;
        }
        #endregion

        #region 切换数据库
        /// <summary>
        /// 通用Conn，可切换数据库Tidb、Mysql、Mariadb、PostgreSql、SqlServer，修改配置DbType，
        /// sql语句CRUD及分页语句完全兼容
        /// </summary>
        public static DbConnection Conn()
        {
            //var dbtype = ConfigA.DbType.LowerA();
            if (dbtype == "tidb" || dbtype == "mysql" || dbtype == "mariadb")
                return ConnMySql();

            if (dbtype == "sqlserver")
                return ConnSqlServer();

            //需要安装nuget包 Install-Package Npgsql
            //if (dbtype == "postgresql") 
            //    return ConnPostgreSQL(); 

            return ConnMySql();
        }

        /// <summary>
        /// 异步，通用Conn，可切换数据库Tidb、Mysql、Mariadb、PostgreSql、SqlServer，修改配置DbType，
        /// sql语句CRUD及分页语句完全兼容
        /// </summary>
        public static async Task<DbConnection> ConnAsync()
        {
            //var dbtype = ConfigA.DbType.LowerA();
            if (dbtype == "tidb" || dbtype == "mysql" || dbtype == "mariadb")
                return await ConnMySqlAsync();

            if (dbtype == "sqlserver")
                return await ConnSqlServerAsync();

            //需要安装nuget包 Install-Package Npgsql
            //if (dbtype == "postgresql") 
            //    return ConnPostgreSQL(); 

            return await ConnMySqlAsync();
        }
        #endregion
    }
}
