using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LexicalAnalysis.DDL
{
    public class DDLParser
    {
        private List<DDLResult> results { get; set; } = new List<DDLResult>();
        private int id;
        private DDLResult initToken(char value, DDLResult result, Status pStatus)
        {
            if (result.text.Length > 0)
            {
                results.Add(result);
                result = new DDLResult();
            }

            if (value == 'C' && pStatus == Status.BASE_INIT)
            {
                result.tokenType = DDLTokenType.CT;
                result.text.Append(value);
            }
            else if (value == '`' && pStatus == Status.BASE_INIT)
            {
                result.tokenType = DDLTokenType.FI;
                result.text.Append(value);
            }
            else if (value == '`' && pStatus == Status.BASE_CRT)
            {
                result.tokenType = DDLTokenType.TBN;
                //result.text.Append(value);
            }
            else if (value == '`' && pStatus == Status.BASE_FIELD_NAME)
            {
                result.tokenType = DDLTokenType.FIELD_NAME;
            }
            else if (value == ' ' && pStatus == Status.BASE_FIELD_TYPE)
            {
                result.tokenType = DDLTokenType.FIELD_TYPE;
                result.text.Append(value);
            }
            else if (value == '(' && pStatus == Status.BASE_FIELD_LEN)
            {
                result.tokenType = DDLTokenType.FIELD_LEN;
            }
            else if (value == '\'' && pStatus == Status.BASE_FIELD_COMMENT)
            {
                result.tokenType = DDLTokenType.FIELD_COMMENT;
            }
            else if (value == 'P' && pStatus == Status.BASE_INIT)
            {
                result.tokenType = DDLTokenType.P_K;
                result.text.Append(value);
            }
            else if (value == '`' && pStatus == Status.BASE_FIELD_PK)
            {
                result.tokenType = DDLTokenType.P_K_V;
            }
            else if (value == 'K' && pStatus == Status.BASE_INIT)
            {
                result.tokenType = DDLTokenType.K;
                result.text.Append(value);
            }
            else
            {
                result.tokenType = DDLTokenType.INIT;
            }
            return result;
        }



        public List<DDLResult> tokenize(string script, Status pStatus, int pid)
        {
            var reader = new StringReader(script);
            var status = DDLTokenType.INIT;
            int ch;
            char value;
            var result = new DDLResult();
            while ((ch = reader.Read()) != -1)
            {
                value = (char)ch;
                switch (status)
                {
                    case DDLTokenType.INIT:
                        result = initToken(value, result, pStatus);
                        status = result.tokenType;
                        break;
                    case DDLTokenType.CT:
                        if (value == '\n')
                        {
                            status = DDLTokenType.INIT;

                            // 继续解析 CREATE TABLE `t` 中的 t
                            var newStatus = result.setStatus(Status.BASE_CRT);
                            tokenize(result.text.ToString(), newStatus, result.pid);
                        }
                        else
                        {
                            result.text.Append(value);
                        }
                        break;
                    case DDLTokenType.FI:
                        //&& reader.Peek() == '\n'
                        if (value == ',')
                        {
                            status = DDLTokenType.INIT;
                            result.pid = nextPid();

                            //继续解析 `name` varchar(50) DEFAULT NULL COMMENT '终端机名称' 中的 `name`
                            var newStatus = result.setStatus(Status.BASE_FIELD_NAME);
                            tokenize(result.text.ToString(), newStatus, result.pid);

                            //继续解析 `name` varchar(50) DEFAULT NULL COMMENT '终端机名称' 中的 varchar
                            newStatus = result.setStatus(Status.BASE_FIELD_TYPE);
                            tokenize(result.text.ToString(), newStatus, result.pid);

                            //继续解析 `name` varchar(50) DEFAULT NULL COMMENT '终端机名称' 中的 50
                            newStatus = result.setStatus(Status.BASE_FIELD_LEN);
                            tokenize(result.text.ToString(), newStatus, result.pid);

                            //继续解析 `name` varchar(50) DEFAULT NULL COMMENT '终端机名称' 中的 '终端机名称'
                            newStatus = result.setStatus(Status.BASE_FIELD_COMMENT);
                            tokenize(result.text.ToString(), newStatus, result.pid);
                        }
                        else
                        {
                            result.text.Append(value);
                        }
                        break;
                    case DDLTokenType.TBN:
                        if (value == '`')
                        {
                            status = DDLTokenType.INIT;
                            result.pid = pid;
                        }
                        else
                        {
                            result.text.Append(value);
                        }
                        break;
                    case DDLTokenType.FIELD_NAME:
                        if (value == '`')
                        {
                            status = DDLTokenType.INIT;
                            result.pid = pid;
                        }
                        else
                        {
                            result.text.Append(value);
                        }
                        break;
                    case DDLTokenType.FIELD_TYPE:
                        // 解析 varchar(50) 为 varchar，所以只能以 ( 结尾
                        if (value == '(')
                        {
                            status = DDLTokenType.INIT;
                            result.pid = pid;
                        }
                        else if (value == ' ')
                        {
                            // 兼容 `create_time` datetime   这类数据（datetime是以空格结尾）
                            status = DDLTokenType.INIT;
                            result.pid = pid;
                        }
                        else
                        {
                            if (isNotFieldType(value))
                            {
                                status = DDLTokenType.INIT;
                            }
                            else
                            {
                                result.text.Append(value);
                            }
                        }
                        break;
                    case DDLTokenType.FIELD_LEN:
                        if (value == ')')
                        {
                            status = DDLTokenType.INIT;
                            result.pid = pid;
                        }
                        else
                        {
                            result.text.Append(value);
                        }
                        break;
                    case DDLTokenType.FIELD_COMMENT:
                        if (value == '\'')
                        {
                            status = DDLTokenType.INIT;
                            result.pid = pid;
                        }
                        else
                        {
                            result.text.Append(value);
                        }
                        break;
                    case DDLTokenType.P_K:
                        if (value == ')')
                        {
                            result.text.Append(value);
                            status = DDLTokenType.INIT;
                            result.pid = nextPid();

                            // 继续解析 PRIMARY KEY (`id`)--->id
                            var newStatus = result.setStatus(Status.BASE_FIELD_PK);
                            tokenize(result.text.ToString(), newStatus, result.pid);
                        }
                        break;
                    case DDLTokenType.P_K_V:
                        if (value == '`')
                        {
                            status = DDLTokenType.INIT;
                            result.pid = pid;
                        }
                        else
                        {
                            result.text.Append(value);
                        }
                        break;
                    case DDLTokenType.K:
                        if (value == '\n')
                        {
                            status = DDLTokenType.INIT;
                        }
                        else
                        {
                            result.text.Append(value);
                        }
                        break;
                }
            }
            if (result.text.Length > 0)
            {
                results.Add(result);
            }
            return results;
        }


        /// <summary>
        /// 不属于 fieldType 的字符，NOT NULL AUTO_INCREMENT COMMENT SET utf8mb4 DEFAULT NULL COMMENT
        /// </summary>
        private string notFieldType = "' N A C S u D";

        private bool isNotFieldType(char value)
        {
            foreach (var c in notFieldType.Trim())
            {
                if (c == value)
                {
                    return true;
                }
            }
            return false;
        }

        private int nextPid()
        {
            return ++id;
        }
    }
}
