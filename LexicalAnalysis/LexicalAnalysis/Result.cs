using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LexicalAnalysis
{
    public class Result
    {
        public TokenType tokenType { get; set; }
        public StringBuilder text { get; set; } = new StringBuilder();
    }
}
