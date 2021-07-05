using System;
using Zaabee.MsgBus.DapperExtensions.Enums;

namespace Zaabee.MsgBus.DapperExtensions.Adapters
{
    internal class PostgresAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName)
        {
            return $"\"{columnName}\"";
        }

        protected override string CriteriaTypeStringParse(TypeMapInfo typeMapInfo, CriteriaType criteriaType)
        {
            return criteriaType switch
            {
                CriteriaType.None => string.Empty,
                CriteriaType.SingleId => $"WHERE {typeMapInfo.TableName}.{typeMapInfo.IdColumnName} = @Id",
                CriteriaType.MultiId => $"WHERE {typeMapInfo.TableName}.{typeMapInfo.IdColumnName} = ANY(@Ids)",
                _ => throw new ArgumentOutOfRangeException(nameof(criteriaType), criteriaType, null)
            };
        }
    }
}