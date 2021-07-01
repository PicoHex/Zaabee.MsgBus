namespace Zaabee.MsgBus.DapperExtensions.Adapters
{
    internal class SqlServerAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName)
        {
            return $"[{columnName}]";
        }
    }
}