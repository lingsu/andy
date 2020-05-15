using System;
using System.Collections.Generic;
using System.Text;

namespace LexicalAnalysis.Mssql
{
    public enum Status
    {
        BASE_INIT,
        DBO,
        /// <summary>
        /// 表明
        /// </summary>
        TBN,
        /// <summary>
        /// 字段信息
        /// </summary>
        FI,
        FIELD_NAME,
        FIELD_TYPE,
        FIELD_LEN,
        FIELD_COMMENT,
    }
}
