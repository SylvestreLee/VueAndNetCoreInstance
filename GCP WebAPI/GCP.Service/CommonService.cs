using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GCP.Code;
using GCP.Repository;
using System.Linq;
using GCP.Util.Extension;
using static GCP.Model.CommonModel;
using GCP.Util;

namespace GCP.Service
{
    public class CommonService
    {
        private readonly IFreeSql sql = Sql.GetFsql();

        #region 拓展SQL方法

        public async Task<List<T>> GetList<T>(string strSql) where T : class, new()
        {
            return await sql.Ado.QueryAsync<T>(strSql);
        }

        public async Task<List<T>> GetList<T>(string strSql, List<DbParameter> filter) where T : class, new()
        {
            return await sql.Ado.QueryAsync<T>(strSql, filter);
        }

        public async Task<System.Data.DataTable> GetDataTable(string strSql)
        {
            return await sql.Ado.ExecuteDataTableAsync(strSql);
        }

        public async Task<System.Data.DataTable> GetDataTable(string strSql, List<DbParameter> filter)
        {
            return await sql.Ado.ExecuteDataTableAsync(strSql, filter);
        }

        /// <summary>
        /// 获取枚举表内容
        /// </summary>
        /// <param name="EnumTableName">枚举表名称</param>
        /// <param name="SqlWhere">枚举表查询条件</param>
        /// <param name="EnumIDColumnName">枚举表ID字段别名</param>
        /// <param name="EnumNameColumnName">枚举表Name字段别名</param>
        /// <returns></returns>
        public async Task<List<EnumIdNameModel>> GetEnumList(string EnumTableName, string SqlWhere = "", string EnumIDColumnName = "ID", string EnumNameColumnName = "Name")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  " + EnumIDColumnName + @",
                                    " + EnumNameColumnName + @"
                            FROM    " + EnumTableName + @"
                            ");
            if (SqlWhere != "")
            {
                strSql.Append(" WHERE " + SqlWhere);
            }

            var dt = await GetDataTable(strSql.ToString());

            return dt.DtToListWithNoName<EnumIdNameModel>();
        }

        #endregion

        
        #region 表达式相关拓展方法

        private Expression Contains_result(Expression left, Expression right)
        {
            var stringContainsMethod = typeof(CommonService).GetMethod("StringsContains");
            return Expression.Call(left, stringContainsMethod, right); ;
        }

        public static bool StringsContains(string value, string subValue)
        {
            if (value == null)
            {
                return false;
            }
            return value.Contains(subValue);
        }

        private Expression Equal_result(Expression left, Expression right)
        {
            return Expression.Equal(left, right);
        }

        private Expression<Func<T, bool>> ExpressionEqual<T>(string columnName, object value)
        {
            var filter = Expression.Equal(Expression.Constant(1), Expression.Constant(1));
            var param = Expression.Parameter(typeof(T));
            Expression[] exs = new Expression[1];
            Expression left = Expression.Property(param, typeof(T).GetProperty(columnName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Equal_result(left, right);
            exs[0] = result;
            filter = Expression.AndAlso(filter, exs[0]);// where 条件 拼接
            return Expression.Lambda<Func<T, bool>>(filter, param);
        }

        //private Expression<Func<T, T2, bool>> ExpressionEqual<T, T2>(string columnName, object value)
        //{
        //    var filter = Expression.Equal(Expression.Constant(1), Expression.Constant(1));

        //    ParameterExpression[] ALLParams = new ParameterExpression[2];
        //    ALLParams[0] = Expression.Parameter(typeof(T));
        //    ALLParams[1] = Expression.Parameter(typeof(T2));

        //    Expression[] exs = new Expression[1];
        //    Expression left = Expression.Property(Expression.Parameter(typeof(StationEntity)), typeof(StationEntity).GetProperty(columnName));
        //    Expression right = Expression.Constant(value, value.GetType());
        //    Expression result = Equal_result(left, right);
        //    exs[0] = result;
        //    filter = Expression.AndAlso(filter, exs[0]);// where 条件 拼接
        //    return Expression.Lambda<Func<T, T2, bool>>(filter, ALLParams);
        //}

        //private Expression<Func<T, T2, T3, bool>> ExpressionEqual<T, T2, T3>(string columnName, object value)
        //{
        //    var filter = Expression.Equal(Expression.Constant(1), Expression.Constant(1));

        //    ParameterExpression[] ALLParams = new ParameterExpression[3];
        //    ALLParams[0] = Expression.Parameter(typeof(T));
        //    ALLParams[1] = Expression.Parameter(typeof(T2));
        //    ALLParams[2] = Expression.Parameter(typeof(T3));

        //    Expression[] exs = new Expression[1];
        //    Expression left = Expression.Property(Expression.Parameter(typeof(StationEntity)), typeof(StationEntity).GetProperty(columnName));
        //    Expression right = Expression.Constant(value, value.GetType());
        //    Expression result = Equal_result(left, right);
        //    exs[0] = result;
        //    filter = Expression.AndAlso(filter, exs[0]);// where 条件 拼接
        //    return Expression.Lambda<Func<T, T2, T3, bool>>(filter, ALLParams);
        //}

        //private Expression<Func<T, T2, T3, T4, bool>> ExpressionEqual<T, T2, T3, T4>(string columnName, object value)
        //{
        //    var filter = Expression.Equal(Expression.Constant(1), Expression.Constant(1));

        //    ParameterExpression[] ALLParams = new ParameterExpression[4];
        //    ALLParams[0] = Expression.Parameter(typeof(T));
        //    ALLParams[1] = Expression.Parameter(typeof(T2));
        //    ALLParams[2] = Expression.Parameter(typeof(T3));
        //    ALLParams[3] = Expression.Parameter(typeof(T4));

        //    Expression[] exs = new Expression[1];
        //    Expression left = Expression.Property(Expression.Parameter(typeof(StationEntity)), typeof(StationEntity).GetProperty(columnName));
        //    Expression right = Expression.Constant(value, value.GetType());
        //    Expression result = Equal_result(left, right);
        //    exs[0] = result;
        //    filter = Expression.AndAlso(filter, exs[0]);// where 条件 拼接
        //    return Expression.Lambda<Func<T, T2, T3, T4, bool>>(filter, ALLParams);
        //}

        //private Expression<Func<T, T2, T3, T4, T5, bool>> ExpressionEqual<T, T2, T3, T4, T5>(string columnName, object value)
        //{
        //    var filter = Expression.Equal(Expression.Constant(1), Expression.Constant(1));

        //    ParameterExpression[] ALLParams = new ParameterExpression[5];
        //    ALLParams[0] = Expression.Parameter(typeof(T));
        //    ALLParams[1] = Expression.Parameter(typeof(T2));
        //    ALLParams[2] = Expression.Parameter(typeof(T3));
        //    ALLParams[3] = Expression.Parameter(typeof(T4));
        //    ALLParams[4] = Expression.Parameter(typeof(T5));

        //    Expression[] exs = new Expression[1];
        //    Expression left = Expression.Property(Expression.Parameter(typeof(StationEntity)), typeof(StationEntity).GetProperty(columnName));
        //    Expression right = Expression.Constant(value, value.GetType());
        //    Expression result = Equal_result(left, right);
        //    exs[0] = result;
        //    filter = Expression.AndAlso(filter, exs[0]);// where 条件 拼接
        //    return Expression.Lambda<Func<T, T2, T3, T4, T5, bool>>(filter, ALLParams);
        //}

        private Expression<Func<T, bool>> ExpressionStationContains<T>(string columnName, object[] values)
        {
            var filter = Expression.Equal(Expression.Constant(1), Expression.Constant(1));
            var param = Expression.Parameter(typeof(T));
            Expression[] exs = new Expression[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                var v = values[i];
                Expression left = Expression.Property(param, typeof(T).GetProperty(columnName));
                Expression right = Expression.Constant(v, v.GetType());
                Expression result = Equal_result(left, right);
                exs[i] = result;
            }
            if (exs.Length == 1)
            {
                filter = Expression.AndAlso(filter, exs[0]);// where 条件 拼接
            }
            else if (exs.Length > 1)
            {
                filter = Expression.OrElse(exs[0], exs[1]);
                for (var i = 2; i < exs.Length; i++)
                {
                    filter = Expression.OrElse(filter, exs[i]);
                }
            }
            return Expression.Lambda<Func<T, bool>>(filter, param);
        }

        //private Expression<Func<T, T2, bool>> ExpressionStationContains<T, T2>(string columnName, object[] values)
        //{
        //    var filter = Expression.Equal(Expression.Constant(1), Expression.Constant(1));

        //    ParameterExpression[] ALLParams = new ParameterExpression[2];
        //    ALLParams[0] = Expression.Parameter(typeof(T));
        //    ALLParams[1] = Expression.Parameter(typeof(T2));
        //    Expression[] exs = new Expression[values.Length];
        //    for (int i = 0; i < values.Length; i++)
        //    {
        //        var v = values[i];
        //        Expression left = Expression.Property(Expression.Parameter(typeof(StationEntity)), typeof(StationEntity).GetProperty(columnName));
        //        Expression right = Expression.Constant(v, v.GetType());
        //        Expression result = Equal_result(left, right);
        //        exs[i] = result;
        //    }
        //    if (exs.Length == 1)
        //    {
        //        filter = Expression.AndAlso(filter, exs[0]);// where 条件 拼接
        //    }
        //    else if (exs.Length > 1)
        //    {
        //        filter = Expression.OrElse(exs[0], exs[1]);
        //        for (var i = 2; i < exs.Length; i++)
        //        {
        //            filter = Expression.OrElse(filter, exs[i]);
        //        }
        //    }
        //    return Expression.Lambda<Func<T, T2, bool>>(filter, ALLParams);
        //}

        //private Expression<Func<T, T2, T3, bool>> ExpressionStationContains<T, T2, T3>(string columnName, object[] values)
        //{
        //    var filter = Expression.Equal(Expression.Constant(1), Expression.Constant(1));

        //    ParameterExpression[] ALLParams = new ParameterExpression[3];
        //    ALLParams[0] = Expression.Parameter(typeof(T));
        //    ALLParams[1] = Expression.Parameter(typeof(T2));
        //    ALLParams[2] = Expression.Parameter(typeof(T3));
        //    Expression[] exs = new Expression[values.Length];
        //    for (int i = 0; i < values.Length; i++)
        //    {
        //        var v = values[i];
        //        Expression left = Expression.Property(Expression.Parameter(typeof(StationEntity)), typeof(StationEntity).GetProperty(columnName));
        //        Expression right = Expression.Constant(v, v.GetType());
        //        Expression result = Equal_result(left, right);
        //        exs[i] = result;
        //    }
        //    if (exs.Length == 1)
        //    {
        //        filter = Expression.AndAlso(filter, exs[0]);// where 条件 拼接
        //    }
        //    else if (exs.Length > 1)
        //    {
        //        filter = Expression.OrElse(exs[0], exs[1]);
        //        for (var i = 2; i < exs.Length; i++)
        //        {
        //            filter = Expression.OrElse(filter, exs[i]);
        //        }
        //    }
        //    return Expression.Lambda<Func<T, T2, T3, bool>>(filter, ALLParams);
        //}

        //private Expression<Func<T, T2, T3, T4, bool>> ExpressionStationContains<T, T2, T3, T4>(string columnName, object[] values)
        //{
        //    var filter = Expression.Equal(Expression.Constant(1), Expression.Constant(1));

        //    ParameterExpression[] ALLParams = new ParameterExpression[4];
        //    ALLParams[0] = Expression.Parameter(typeof(T));
        //    ALLParams[1] = Expression.Parameter(typeof(T2));
        //    ALLParams[2] = Expression.Parameter(typeof(T3));
        //    ALLParams[3] = Expression.Parameter(typeof(T4));
        //    Expression[] exs = new Expression[values.Length];
        //    for (int i = 0; i < values.Length; i++)
        //    {
        //        var v = values[i];
        //        Expression left = Expression.Property(Expression.Parameter(typeof(StationEntity)), typeof(StationEntity).GetProperty(columnName));
        //        Expression right = Expression.Constant(v, v.GetType());
        //        Expression result = Equal_result(left, right);
        //        exs[i] = result;
        //    }
        //    if (exs.Length == 1)
        //    {
        //        filter = Expression.AndAlso(filter, exs[0]);// where 条件 拼接
        //    }
        //    else if (exs.Length > 1)
        //    {
        //        filter = Expression.OrElse(exs[0], exs[1]);
        //        for (var i = 2; i < exs.Length; i++)
        //        {
        //            filter = Expression.OrElse(filter, exs[i]);
        //        }
        //    }
        //    return Expression.Lambda<Func<T, T2, T3, T4, bool>>(filter, ALLParams);
        //}

        //private Expression<Func<T, T2, T3, T4, T5, bool>> ExpressionStationContains<T, T2, T3, T4, T5>(string columnName, object[] values)
        //{
        //    var filter = Expression.Equal(Expression.Constant(1), Expression.Constant(1));

        //    ParameterExpression[] ALLParams = new ParameterExpression[5];
        //    ALLParams[0] = Expression.Parameter(typeof(T));
        //    ALLParams[1] = Expression.Parameter(typeof(T2));
        //    ALLParams[2] = Expression.Parameter(typeof(T3));
        //    ALLParams[3] = Expression.Parameter(typeof(T4));
        //    ALLParams[4] = Expression.Parameter(typeof(T5));
        //    Expression[] exs = new Expression[values.Length];
        //    for (int i = 0; i < values.Length; i++)
        //    {
        //        var v = values[i];
        //        Expression left = Expression.Property(Expression.Parameter(typeof(StationEntity)), typeof(StationEntity).GetProperty(columnName));
        //        Expression right = Expression.Constant(v, v.GetType());
        //        Expression result = Equal_result(left, right);
        //        exs[i] = result;
        //    }
        //    if (exs.Length == 1)
        //    {
        //        filter = Expression.AndAlso(filter, exs[0]);// where 条件 拼接
        //    }
        //    else if (exs.Length > 1)
        //    {
        //        filter = Expression.OrElse(exs[0], exs[1]);
        //        for (var i = 2; i < exs.Length; i++)
        //        {
        //            filter = Expression.OrElse(filter, exs[i]);
        //        }
        //    }
        //    return Expression.Lambda<Func<T, T2, T3, T4, T5, bool>>(filter, ALLParams);
        //}

        #endregion

        
    }
}
