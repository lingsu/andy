using System;
using System.Collections.Generic;
using System.Text;

namespace LexicalAnalysis.DDL
{
    public class DDLResult
    {
        public DDLTokenType tokenType { get; set; }
        public int pid { get; set; }
        public Status status { get; private set; }
        public StringBuilder text { get; set; } = new StringBuilder();

        public Status setStatus(Status status)
        {
            this.status = status;
            return status;
        }
    }
}
