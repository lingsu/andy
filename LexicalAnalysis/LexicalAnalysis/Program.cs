using LexicalAnalysis.DDL;
using LexicalAnalysis.Mssql;
using System;

namespace LexicalAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            //One();
            Two();
            Three();
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
                "  `xx4_statusC` varchar(64) NOT NULL COMMENT '我 HH 态',\n" +
                "  PRIMARY KEY (`id`)\n" +
                ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='xx表';";

            var tokenize = new DDLParser().tokenize(sql, DDL.Status.BASE_INIT, 0);
            Console.WriteLine("base \ttoken \t value \t pid");

            foreach (var result in tokenize)
            {
                Console.WriteLine(result.status + "\t" + result.tokenType + " \t " + result.text.ToString() + "\t" + result.pid);
            }
        }

        static void Three()
        {
            var sql = @"CREATE TABLE [dbo].[AbpRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ConcurrencyStamp] [nvarchar](128) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
	[DisplayName] [nvarchar](64) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsStatic] [bit] NOT NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[Name] [nvarchar](32) NOT NULL,
	[NormalizedName] [nvarchar](32) NOT NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_AbpRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]";

            var sql2 = @"CREATE TABLE [dbo].[account_onu](
	[update_time] [datetime] NOT NULL,
	[account] [varchar](50) NULL,
	[olt_ip] [varchar](15) NULL,
	[onu_ip] [varchar](15) NULL,
	[slot] [int] NULL,
	[port] [int] NULL,
	[ont_id] [int] NULL,
	[mac_type] [int] NULL,
	[bras_name] [nvarchar](250) NULL,
	[bras_ip] [nvarchar](250) NULL,
	[bras_trunk] [nvarchar](250) NULL,
	[onu_passwd] [nvarchar](250) NULL,
	[onu_sn] [nvarchar](250) NULL
) ON [PRIMARY]";

            var tableInfo = new MssqlParser(sql2).GenerateDDLInfo();
            //Console.WriteLine("base \ttoken \t value \t pid");

            //foreach (var result in tokenize)
            //{
            //    Console.WriteLine(result.Status + "\t" + result.TokenType + " \t " + result.Text.ToString() + "\t" + result.Pid);
            //}
            //Console.WriteLine("base \ttoken \t value \t pid");
            Console.WriteLine("TableName:" + tableInfo.TableName);
            foreach (var result in tableInfo.Col)
            {
                Console.WriteLine(result.Name + "\t" + result.Type + " \t " + result.IsNull + "\t" + result.IsK);
            }

        }
    }
}
