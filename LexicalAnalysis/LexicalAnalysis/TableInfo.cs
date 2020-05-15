using System;
using System.Collections.Generic;
using System.Text;

namespace LexicalAnalysis
{
    public class TableInfo
    {
        public string TableName { get; set; }
        public List<TableCol> Col { get; set; } = new List<TableCol>();
        public void AddCol(string name, string type, bool isNull, bool isK)
        {
            Col.Add(new TableCol()
            {
                Name = name,
                Type = type,
                IsNull = isNull,
                IsK = isK
            });
        }
    }
    public class TableCol
    {
        public string Name { get; set; }
        public bool IsNull { get; set; }
        public string Type { get; set; }
        public bool IsK { get; set; }
    }
}
