using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LexicalAnalysis.Mssql
{
    public class MssqlParser
    {
        private List<Result> results { get; set; } = new List<Result>();
        private int id;
        private string script { get; set; }

        public MssqlParser(string script)
        {
            this.script = script;
        }

        public List<Result> Tokenize(string script, Status status, int pid)
        {
            var reader = new StringReader(script);
            var result = new Result();
            var type = TokenType.INIT;
            var lastType = TokenType.INIT;
            char value;
            int ch;
            while ((ch = reader.Read()) != -1)
            {
                value = (char)ch;
                switch (type)
                {
                    case TokenType.INIT:
                        result = initToken(value, result, status, lastType);
                        type = result.TokenType;

                        break;
                    case TokenType.CT:
                        if (value == '(')
                        {
                            result.Pid = nextPid();
                            type = TokenType.INIT;
                            lastType = TokenType.CT;
                            Tokenize(result.Text.ToString(), Status.DBO, result.Pid);
                            //Tokenize(result.Text.ToString(), Status.TBN, result.Pid);
                        }
                        else
                        {
                            result.Text.Append(value);
                        }

                        break;
                    case TokenType.DBO:
                        if (value == ']')
                        {
                            type = TokenType.INIT;
                            result.Pid = pid;
                            lastType = TokenType.DBO;
                        }
                        else
                        {
                            result.Text.Append(value);
                        }
                        break;
                    case TokenType.TBN:
                        if (value == ']')
                        {
                            type = TokenType.INIT;
                            result.Pid = pid;
                            lastType = TokenType.INIT;
                        }
                        else
                        {
                            result.Text.Append(value);
                        }
                        break;
                    case TokenType.FI:
                        if ((value == ',' && (new char[] { '\n', '\r' }).Contains((char)reader.Peek())) || value == '\n')
                        {
                            //var ff = && reader.Peek() == '\n'
                            type = TokenType.INIT;

                            result.Pid = nextPid();
                            lastType = TokenType.FI;
                            Tokenize(result.Text.ToString(), Status.FI, result.Pid);
                        }
                        else
                        {
                            result.Text.Append(value);
                        }
                        break;
                    case TokenType.FIELD_NAME:
                        if (value == ']')
                        {
                            type = TokenType.INIT;
                            result.Pid = pid;
                            lastType = TokenType.FIELD_NAME;
                        }
                        else
                        {
                            result.Text.Append(value);
                        }
                        break;
                    case TokenType.FIELD_TYPE:
                        if (value == ']')
                        {
                            type = TokenType.INIT;
                            result.Pid = pid;
                            lastType = TokenType.FIELD_TYPE;
                        }
                        else
                        {
                            result.Text.Append(value);
                        }
                        break;
                    case TokenType.FIELD_LEN:
                        if (value == ')')
                        {
                            type = TokenType.INIT;
                            result.Pid = pid;
                        }
                        else
                        {
                            result.Text.Append(value);
                        }
                        break;
                    case TokenType.FIELD_NULL:
                        result.Pid = pid;
                        if (value == ',' || value == '\n')
                        {
                            type = TokenType.INIT;
                        }
                        else
                        {
                            result.Text.Append(value);
                        }

                        break;
                    case TokenType.FIELD_COMMENT:
                        break;
                    case TokenType.P_K:
                        if (value == ')')
                        {
                            type = TokenType.INIT;

                            result.Pid = nextPid();
                            //lastType = TokenType.FI;
                            Tokenize(result.Text.ToString(), Status.P_K, result.Pid);
                        }
                        else
                        {
                            result.Text.Append(value);
                        }
                        break;
                    case TokenType.P_K_V:
                        if (value == '(')
                        {
                            type = TokenType.INIT;
                            result.Pid = pid;
                        }
                        else
                        {
                            result.Text.Append(value);
                        }
                        break;
                    case TokenType.K:
                        if (value == ']')
                        {
                            type = TokenType.INIT;
                            result.Pid = pid;
                        }
                        else
                        {
                            result.Text.Append(value);
                        }

                        break;

                    default:
                        break;
                }
            }

            if (result.Text.Length > 0)
            {
                results.Add(result);
            }

            return results;
        }

        private Result initToken(char value, Result result, Status pStatus, TokenType lastType)
        {
            if (result.Text.Length > 0)
            {
                results.Add(result);
                result = new Result();
            }

            if (value == 'C' && pStatus == Status.BASE_INIT  && lastType == TokenType.FI)
            {
                result.TokenType = TokenType.P_K;
                result.Text.Append(value);
            }
            else if (value == 'C' && pStatus == Status.BASE_INIT)
            {
                result.TokenType = TokenType.CT;
                result.Text.Append(value);
            }
            else if (value == 'C' && pStatus == Status.P_K)
            {
                result.TokenType = TokenType.P_K_V;
                result.Text.Append(value);
            }
            else if (value == '[' && pStatus == Status.DBO && lastType == TokenType.INIT)
            {
                result.TokenType = TokenType.DBO;
            }
            else if (value == '[' && lastType == TokenType.DBO)
            {
                result.TokenType = TokenType.TBN;
            }
            else if (value == '[' && pStatus == Status.BASE_INIT)
            {
                result.TokenType = TokenType.FI;
                result.Text.Append(value);
            }
            else if (value == '[' && pStatus == Status.P_K)
            {
                result.TokenType = TokenType.K;
            }
            else if (value == '[' && pStatus == Status.FI && lastType == TokenType.INIT)
            {
                result.TokenType = TokenType.FIELD_NAME;
            }
            else if (value == '[' && pStatus == Status.FI && lastType == TokenType.FIELD_NAME)
            {
                result.TokenType = TokenType.FIELD_TYPE;
            }
            else if (value == '(' && pStatus == Status.FI && lastType == TokenType.FIELD_TYPE)
            {
                result.TokenType = TokenType.FIELD_LEN;
            }
            else if (value == 'N' && pStatus == Status.FI && lastType == TokenType.FIELD_TYPE)
            {
                result.TokenType = TokenType.FIELD_NULL;
                result.Text.Append(value);
            }
            return result;
        }
        private int nextPid()
        {
            return ++id;
        }
        public TableInfo GenerateDDLInfo()
        {
            var tableinfo = new TableInfo();
            var result = Tokenize(script, Mssql.Status.BASE_INIT, 0);
            Console.WriteLine("base \ttoken \t value \t pid");

            foreach (var tokenize in result)
            {
                Console.WriteLine(tokenize.Status + "\t" + tokenize.TokenType + " \t " + tokenize.Text.ToString() + "\t" + tokenize.Pid);
            }

            tableinfo.TableName = result.First(x => x.TokenType == TokenType.TBN).NodeText;
            foreach (var item in result.GroupBy(x=>x.Pid))
            {
                if (item.Any(x=>x.TokenType == TokenType.FI) && item.Any(x => x.TokenType == TokenType.FIELD_NAME) && item.Any(x => x.TokenType == TokenType.FIELD_TYPE))
                {
                    var name = item.First(x => x.TokenType == TokenType.FIELD_NAME).NodeText;

                    tableinfo.AddCol(name,
                        item.First(x => x.TokenType == TokenType.FIELD_TYPE).NodeText,
                        item.FirstOrDefault(x => x.TokenType == TokenType.FIELD_NULL)?.NodeText == "NULL",
                        result.Any(x=>x.NodeText == name && x.TokenType == TokenType.K)
                        );
                }
            }
            return tableinfo;
        }
    }
}
