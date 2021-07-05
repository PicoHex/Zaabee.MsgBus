namespace Zaabee.MsgBus.DapperExtensions.Adapters
{
    internal class SQLiteAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName)
        {
            return $"\"{columnName}\"";
        }
    }
}