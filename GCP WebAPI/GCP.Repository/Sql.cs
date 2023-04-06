using GCP.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Repository
{
    public class Sql
    {
        private static IFreeSql _freeSql;
        private static readonly object locker = new object();

        public static IFreeSql GetFsql()
        {
            string DbType = GlobalContext.SystemConfig.DBProvider;
            FreeSql.DataType dataType = FreeSql.DataType.SqlServer;
            switch (DbType)
            {
                case "MySql":
                    dataType = FreeSql.DataType.MySql;
                    break;
                case "SqlServer":
                    dataType = FreeSql.DataType.SqlServer;
                    break;
                case "PostgreSQL":
                    dataType = FreeSql.DataType.PostgreSQL;
                    break;
                case "Oracle":
                    dataType = FreeSql.DataType.Oracle;
                    break;
                case "Sqlite":
                    dataType = FreeSql.DataType.Sqlite;
                    break;
                case "Odbc":
                    dataType = FreeSql.DataType.Odbc;
                    break;
                case "MsAccess":
                    dataType = FreeSql.DataType.MsAccess;
                    break;
                case "Dameng":
                    dataType = FreeSql.DataType.Dameng;
                    break;
                case "OdbcKingbaseES":
                    dataType = FreeSql.DataType.OdbcKingbaseES;
                    break;
                case "ShenTong":
                    dataType = FreeSql.DataType.ShenTong;
                    break;
                case "KingbaseES":
                    dataType = FreeSql.DataType.KingbaseES;
                    break;
                case "Firebird":
                    dataType = FreeSql.DataType.Firebird;
                    break;
                case "Custom":
                    dataType = FreeSql.DataType.Custom;
                    break;
            }
            if (_freeSql == null)
            {
                lock (locker)
                {
                    if (_freeSql == null)
                    {
                        _freeSql = new FreeSql.FreeSqlBuilder()
                        .UseConnectionString(dataType, GlobalContext.SystemConfig.DBConnectionString)
                         .UseLazyLoading(true).Build();
                    }
                }
            }
            return _freeSql;
        }
    }
}
