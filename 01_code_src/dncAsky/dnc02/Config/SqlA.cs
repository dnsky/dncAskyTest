using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapper;

namespace Asky
{  
    public partial class SqlA
    {
        /// <summary>
        /// 方便将来扩展自动追加sql参数
        /// </summary>
        public static string GetSelectSql(string sql, string joinA = null)
        {
            var sqlNew = sql;            
            return sqlNew;
        }

        /// <summary>
        /// 方便将来扩展自动追加sql参数
        /// </summary>
        public static string GetDeleteSql(string sql)
        {
            return GetUpdateSql(sql);
        }

        /// <summary>
        /// 方便将来扩展自动追加sql参数
        /// </summary>
        public static string GetUpdateSql(string sql)
        {         
            var sqlNew = sql;            
            return sqlNew;
        }

        /// <summary>
        /// 方便将来扩展自动追加sql参数
        /// </summary>
        public static string GetInsertSql(string sql)
        {
            var sqlNew = sql;          
            return sqlNew;
        }      

        /// <summary>
        /// dapper执行insert、update、delete，
        /// </summary>
        private static async Task<int> ExecuteAsync(Func<string, string> sqlMethod, string sql, object param = null, int? commandTimeout = null)
        {
            using (var conn = await Db.ConnAsync())
            {
                var sqlNew = sqlMethod(sql);
                return await conn.ExecuteAsync(sqlNew, param, null, commandTimeout);
            }
        }

        /// <summary>
        /// 返回影响的行数
        /// </summary>    
        public static async Task<int> Delete(string sql, object param = null, int? commandTimeout = null)
        {
            return await ExecuteAsync(GetDeleteSql, sql, param, commandTimeout);
        }

        /// <summary>
        /// 返回影响的行数，更新成功返回更新的行数，再次执行也会返回更新行数，配置连接串useAffectedRows=false;
         /// </summary>    
        public static async Task<int> Update(string sql, object param = null, int? commandTimeout = null)
        {
            return await ExecuteAsync(GetUpdateSql, sql, param, commandTimeout);
        }

        /// <summary>
        /// 返回影响的行数，插入成功返回插入的行数，
           /// </summary>    
        public static async Task<int> Insert(string sql, object param = null, int? commandTimeout = null)
        {
            return await ExecuteAsync(GetInsertSql, sql, param, commandTimeout);
        }

        /// <summary>
        /// 查询语句，
        /// pageIndex与pageSize为1时表示取出第一条，相当于SqlServer的top 1，
        /// 多表关联查询时，关联表别名tableA a 则传递joinA参数为a，用于自动追加 a.uid= uid，
        /// 防止joinA字符串sql注入，只允许5个字符以内，例如a，
        /// 分页方法只传递int，没有sql注入风险
        /// </summary>
        public static async Task<List<T>> Select<T>(string sql, object param = null, int pageIndex = 1, int pageSize = 1, string joinA = null, int? commandTimeout = null)
        {
            using (var conn = await Db.ConnAsync())
            {
                sql = GetSelectSql(sql, joinA) + Db.SqlPage(pageIndex, pageSize); //分页方法只传递int，没有sql注入风险
                return (await conn.QueryAsync<T>(sql, param, null, commandTimeout)).ToList();
            }
        }

        ///// <summary>
        ///// 插入数据，
        ///// msg为错误提示文字
        ///// </summary>
        //public static async Task<Result> InsertResult(string sql, object param = null, string msg = null, int? commandTimeout = null)
        //{
        //    var amount = await ExecuteAsync(GetInsertSql, sql, param, commandTimeout);
        //    return (amount > 0) ? Code.Success(amount) : Code.Fail(msg ?? "插入失败");
        //}

        ///// <summary>
        ///// 更新数据，
        ///// msg为错误提示文字
        ///// </summary>
        //public static async Task<Result> UpdateResult(string sql, object param = null, string msg = null, int? commandTimeout = null)
        //{
        //    var amount = await ExecuteAsync(GetUpdateSql, sql, param, commandTimeout);
        //    return (amount > 0) ? Code.Success(amount) : Code.Fail(msg ?? "更新失败");
        //}

        ///// <summary>
        ///// 删除数据，
        ///// msg为错误提示文字
        ///// </summary>
        //public static async Task<Result> DeleteResult(string sql, object param = null, string msg = null, int? commandTimeout = null)
        //{
        //    var amount = await ExecuteAsync(GetDeleteSql, sql, param, commandTimeout);
        //    return (amount > 0) ? Code.Success(amount) : Code.Fail(msg ?? "删除失败");
        //}
    }
}
