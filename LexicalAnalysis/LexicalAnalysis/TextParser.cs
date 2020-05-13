using System;
using System.Collections.Generic;
using System.IO;

namespace LexicalAnalysis
{
    public class TextParser
    {
        private List<Result> results { get; set; } = new List<Result>();

        private Result initToken(char value, Result result)
        {
            if (result.text.Length > 0)
            {
                results.Add(result);
                result = new Result();
            }
            if (isLetter(value))
            {
                result.tokenType = TokenType.VAR;
                result.text.Append(value);
            }
            else if (value == '=')
            {
                result.tokenType = TokenType.GE;
                result.text.Append(value);
            }
            else if (isDigit(value))
            {
                result.tokenType = TokenType.VAL;
                result.text.Append(value);
            }
            else
            {
                result.tokenType = TokenType.INIT;
            }

            return result;
        }

        public List<Result> tokenize(string script)
        {
            var reader = new StringReader(script);
            var status = TokenType.INIT;
            int ch;
            char value;
            var result = new Result();
            while ((ch = reader.Read()) != -1)
            {
                value = (char)ch;
                switch (status)
                {
                    case TokenType.INIT:
                        result = initToken(value, result);
                        status = result.tokenType;
                        break;
                    case TokenType.VAR:
                        if (isLetter(value))
                        {
                            result.text.Append(value);
                        }
                        else
                        {
                            status = TokenType.INIT;
                        }
                        break;
                    case TokenType.GE:
                        if (value == '=')
                        {
                            result.text.Append(value);
                        }
                        else
                        {
                            status = TokenType.INIT;
                        }
                        break;
                    case TokenType.VAL:
                        if (isDigit(value))
                        {
                            result.text.Append(value);
                        }
                        else
                        {
                            status = TokenType.INIT;
                        }
                        break;
                    default:
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
        /// 是否字母
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool isLetter(char c)
        {
            return c >= 65 && c <= 122;
        }
        /// <summary>
        /// whether digit
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool isDigit(char c)
        {
            return c >= 48 && c <= 57;
        }
    }
}
