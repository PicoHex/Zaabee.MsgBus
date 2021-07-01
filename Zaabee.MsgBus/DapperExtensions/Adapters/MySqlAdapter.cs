namespace Zaabee.MsgBus.DapperExtensions.Adapters
{
    internal class MySqlAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName)
        {
            return $"'{columnName}'";
        }
    }
}