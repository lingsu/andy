using System;
using System.Collections.Generic;
using System.Text;

namespace LexicalAnalysis.DDL
{
    public enum Status
    {
        BASE_INIT,
        /// <summary>
        /// create table
        /// </summary>
        BASE_CRT,
        BASE_FIELD_NAME,
        BASE_FIELD_TYPE,
        BASE_FIELD_LEN,
        BASE_FIELD_COMMENT,
        BASE_FIELD_PK,
    }
}
