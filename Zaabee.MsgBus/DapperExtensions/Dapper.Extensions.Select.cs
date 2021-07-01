using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Zaabee.MsgBus.DapperExtensions.Enums;

namespace Zaabee.MsgBus.DapperExtensions
{
    internal partial class DapperExtensions
    {
        public static T FirstOrDefault<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.QueryFirstOrDefault<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
                new {Id = id}, transaction, commandTimeout, commandType);
        }

        public static IList<T> Get<T>(this IDbConnection connection, IEnumerable ids, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(T), CriteriaType.MultiId);
            return connection.Query<T>(sql, new {Ids = ids}, transaction, buffered, commandTimeout, commandType)
                .ToList();
        }

        public static IList<T> Take<T>(this IDbConnection connection, int count, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var type = typeof(T);
            var sql = adapter.GetSelectSql(type, CriteriaType.None);
            var sb = new StringBuilder(sql.Trim());
            sb.Insert(6, $" TOP {count}").Append($" ORDER BY {TypeMapInfoHelper.GetTypeMapInfo(type).IdColumnName}");
            return connection.Query<T>(sql, null, transaction, buffered, commandTimeout, commandType).ToList();
        }

        public static IList<T> GetAll<T>(this IDbConnection connection, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Query<T>(adapter.GetSelectSql(typeof(T), CriteriaType.None), null, transaction, buffered,
                commandTimeout, commandType).ToList();
        }
    }
}