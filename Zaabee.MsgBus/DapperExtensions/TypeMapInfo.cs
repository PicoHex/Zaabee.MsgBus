using System.Collections.Generic;
using System.Reflection;

namespace Zaabee.MsgBus.DapperExtensions
{
    internal class TypeMapInfo
    {
        public string TableName { get; set; }
        public PropertyInfo IdPropertyInfo { get; set; }
        public string IdColumnName { get; set; }
        public Dictionary<string, PropertyInfo> PropertyColumnDict { get; set; }
    }
}