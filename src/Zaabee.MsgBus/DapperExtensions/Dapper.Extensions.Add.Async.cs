using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Zaabee.MsgBus.DapperExtensions
{
    internal static partial class DapperExtensions
    {
        public static async Task<int> AddAsync<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await AddAsync(connection, persistentObject, typeof(T), transaction, commandTimeout, commandType);

        public static async Task<int> AddAsync(this IDbConnection connection, object persistentObject, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var result = await connection.ExecuteAsync(adapter.GetInsertSql(type), persistentObject, transaction,
                commandTimeout, commandType);
            return result;
        }

        public static async Task<int> AddRangeAsync<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await AddRangeAsync(connection, persistentObjects, typeof(T),
                transaction, commandTimeout, commandType);
        }

        public static async Task<int> AddRangeAsync(this IDbConnection connection, IEnumerable persistentObjects,
            Type type, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var result = await connection.ExecuteAsync(adapter.GetInsertSql(type), persistentObjects, transaction,
                commandTimeout, commandType);
            return result;
        }
    }
}