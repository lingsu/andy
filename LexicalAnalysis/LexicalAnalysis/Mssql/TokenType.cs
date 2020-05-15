using System;
using System.Collections.Generic;
using System.Text;

namespace LexicalAnalysis.Mssql
{
    public enum TokenType
    {
        INIT,
        /// <summary>
        /// 创建信息
        /// </summary>
        CT,
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
        FIELD_NULL,
        FIELD_COMMENT,
        P_K,
        /// <summary>
        /// 主键
        /// </summary>
        P_K_V,
        /// <summary>
        /// 普通索引
        /// </summary>
        K
    }
}
