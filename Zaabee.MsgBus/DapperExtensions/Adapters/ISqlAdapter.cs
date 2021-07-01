using System;
using Zaabee.MsgBus.DapperExtensions.Enums;

namespace Zaabee.MsgBus.DapperExtensions.Adapters
{
    internal interface ISqlAdapter
    {
        string GetInsertSql(Type type);
        string GetDeleteSql(Type type, CriteriaType conditionType);
        string GetUpdateSql(Type type);
        string GetSelectSql(Type type, CriteriaType criteriaType);
    }
}