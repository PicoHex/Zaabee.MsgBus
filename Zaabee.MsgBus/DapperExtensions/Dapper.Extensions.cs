using System.Collections.Generic;
using System.Data;
using System.Linq;
using Zaabee.MsgBus.DapperExtensions.Adapters;

namespace Zaabee.MsgBus.DapperExtensions
{
    internal partial class DapperExtensions
    {
        private static readonly Dictionary<string, ISqlAdapter> AdapterDictionary =
            new()
            {
                ["idbconnection"] = new DefaultSqlAdapter(),
                ["sqlconnection"] = new SqlServerAdapter(),
                ["npgsqlconnection"] = new PostgresAdapter(),
                ["mysqlconnection"] = new MySqlAdapter(),
                ["sqliteconnection"] = new SQLiteAdapter()
            };

        private static ISqlAdapter GetSqlAdapter(IDbConnection dbConnection)
        {
            var connName = dbConnection.GetType().Name.ToLower();
            return AdapterDictionary.ContainsKey(connName)
                ? AdapterDictionary[connName]
                : AdapterDictionary.First().Value;
        }
    }
}