using LexicalAnalysis.DDL;
using System;

namespace LexicalAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            //One();
            Two();
        }

        static void One()
        {
            var textParser = new TextParser();

            var script = "ab = 10";
            var tokenize = textParser.tokenize(script);

            Console.WriteLine("token \t value");
            foreach (var result in tokenize)
            {
                Console.WriteLine(result.tokenType + " \t " + result.text.ToString());
            }
        }

        static void Two()
        {
            var sql = "CREATE TABLE `xx_fee` (\n" +
                "  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,\n" +
                "  `xx4_status` varchar(64) NOT NULL COMMENT '我 HH 态',\n" +
                "  PRIMARY KEY (`id`)\n" +
                ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='xx表';";

            var tokenize = new DDLParser().tokenize(sql, Status.BASE_INIT, 0);
            Console.WriteLine("base \ttoken \t value \t pid");

            foreach (var result in tokenize)
            {
                Console.WriteLine( result.status + "\t"+ result.tokenType + " \t " + result.text.ToString() + "\t" + result.pid);
            }
        }
    }
}
