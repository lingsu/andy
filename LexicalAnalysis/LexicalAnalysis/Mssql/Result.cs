using System;
using System.Collections.Generic;
using System.Text;

namespace LexicalAnalysis.Mssql
{
    public class Result
    {
        public TokenType TokenType { get; set; }
        public int Pid { get; set; }
        public Status Status { get; set; }
        public StringBuilder Text { get; set; } = new StringBuilder();
        public string NodeText { get { return Text.ToString().Trim(); } }
    }
}
