using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Zaabee.MsgBus.DapperExtensions.Enums;

namespace Zaabee.MsgBus.DapperExtensions
{
    internal static partial class DapperExtensions
    {
        public static async Task<int> DeleteAsync<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var id = TypeMapInfoHelper.GetIdValue(persistentObject);
            return await DeleteAsync<T>(connection, id, transaction, commandTimeout, commandType);
        }

        public static async Task<int> DeleteAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(
                adapter.GetDeleteSql(typeof(T), CriteriaType.SingleId),
                new {Id = id}, transaction, commandTimeout, commandType);
        }

        public static async Task<int> DeleteAllAsync<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.SingleId),
                persistentObjects, transaction, commandTimeout, commandType);
        }

        public static async Task<int> DeleteAllAsync<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.MultiId),
                new {Ids = (IEnumerable) ids}, transaction, commandTimeout, commandType);
        }

        public static async Task<int> DeleteAllAsync<T>(this IDbConnection connection,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.None), null, transaction,
                commandTimeout, commandType);
        }
    }
}