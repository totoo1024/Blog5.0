using App.Aop.Log;
using SqlSugar;
using System;
using System.Data;
using System.Text;
using SqlSugarDbType = SqlSugar.DbType;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Core
{
    /// <summary>
    /// 数据库操作上下文
    /// </summary>
    public static class AppDbContext
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// 获取ORM数据库连接对象(只操作数据库一次的使用, 否则会进行多次数据库连接和关闭)
        /// 默认超时时间为30秒
        /// 默认为SQL Server数据库
        /// 默认自动关闭数据库链接, 多次操作数据库请勿使用该属性, 可能会造成性能问题
        /// 要自定义请使用GetIntance()方法或者直接使用Exec方法, 传委托
        /// </summary>
        public static SqlSugarClient Db
        {
            get
            {
                return InitDB(30, SqlSugarDbType.MySql, true);
            }
        }

        /// <summary>
        /// 获得SqlSugarClient(使用该方法, 默认请手动释放资源, 如using(var db = SugarBase.GetIntance()){你的代码}, 如果把isAutoCloseConnection参数设置为true, 则无需手动释放, 会每次操作数据库释放一次, 可能会影响性能, 请自行判断使用)
        /// </summary>
        /// <param name="commandTimeOut">等待超时时间, 默认为30秒 (单位: 秒)</param>
        /// <param name="dbType">数据库类型, 默认为SQL Server</param>
        /// <param name="isAutoCloseConnection">是否自动关闭数据库连接, 默认不是, 如果设置为true, 则会在每次操作完数据库后, 即时关闭, 如果一个方法里面多次操作了数据库, 建议保持为false, 否则可能会引发性能问题</param>
        /// <returns></returns>
        public static SqlSugarClient GetIntance(int commandTimeOut = 30, SqlSugarDbType dbType = SqlSugarDbType.MySql, bool isAutoCloseConnection = false)
        {
            return InitDB(commandTimeOut, dbType, isAutoCloseConnection);
        }

        /// <summary>
        /// 初始化ORM连接对象
        /// </summary>
        /// <param name="commandTimeOut">等待超时时间, 默认为30秒 (单位: 秒)</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="isAutoCloseConnection">是否自动关闭数据库连接, 默认不是, 如果设置为true, 则会在每次操作完数据库后, 即时关闭, 如果一个方法里面多次操作了数据库, 建议保持为false, 否则可能会引发性能问题</param>
        private static SqlSugarClient InitDB(int commandTimeOut = 30, SqlSugarDbType dbType = SqlSugarDbType.MySql, bool isAutoCloseConnection = false)
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConnectionString,
                DbType = dbType,
                InitKeyType = InitKeyType.Attribute,//使用特性识别主键
                IsAutoCloseConnection = isAutoCloseConnection
            });
            db.Ado.CommandTimeOut = commandTimeOut;

            #region 记录SQL日志
            ExecuteSqlLogHandler _executeSqlLog = null;
            db.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
            {
                string par = string.Empty;
                if (pars.Length > 0)
                {
                    Dictionary<string, object> dic = pars.ToDictionary(k => k.ParameterName, v => v.Value);
                    par = JsonConvert.SerializeObject(dic).Replace(":", "=");
                }
                _executeSqlLog = new ExecuteSqlLogHandler(sql, par);
            };
            db.Aop.OnLogExecuted = (sql, pars) => //SQL执行完事件
            {
                _executeSqlLog.WriteLog();
            };

            db.Aop.OnError = (exp) =>//执行SQL 错误事件
            {
                _executeSqlLog.LogInfo.IsFail = 1;
                _executeSqlLog.LogInfo.Massage = exp.Message;
                _executeSqlLog.WriteLog();
            };
            #endregion

            return db;
        }

        /// <summary>
        /// 执行数据库操作
        /// </summary>
        /// <typeparam name="Result">返回值类型 泛型</typeparam>
        /// <param name="func">方法委托</param>
        /// <param name="commandTimeOut">超时时间, 单位为秒, 默认为30秒</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>泛型返回值</returns>
        public static Result Exec<Result>(Func<SqlSugarClient, Result> func, int commandTimeOut = 30, SqlSugarDbType dbType = SqlSugarDbType.MySql)
        {
            if (func == null) throw new Exception("委托为null, 事务处理无意义");
            using (var db = InitDB(commandTimeOut, dbType))
            {
                try
                {
                    return func(db);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// 带事务处理的执行数据库操作
        /// </summary>
        /// <typeparam name="Result">返回值类型 泛型</typeparam>
        /// <param name="func">方法委托</param>
        /// <param name="commandTimeOut">超时时间, 单位为秒, 默认为30秒</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>泛型返回值</returns>
        public static Result ExecTran<Result>(Func<SqlSugarClient, Result> func, int commandTimeOut = 30, SqlSugarDbType dbType = SqlSugarDbType.MySql)
        {
            if (func == null) throw new Exception("委托为null, 事务处理无意义");
            using (var db = InitDB(commandTimeOut, dbType))
            {
                try
                {
                    db.Ado.BeginTran(IsolationLevel.Unspecified);
                    var result = func(db);
                    db.Ado.CommitTran();
                    return result;
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    throw ex;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }
    }
}
