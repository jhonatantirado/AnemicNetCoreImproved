using FluentMigrator;
using System.Reflection;

namespace EnterprisePatterns.Api.Migrations.SQLServer
{
    [Migration(1)]
    public class CustomerTable : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("Database.sql");
        }

        public override void Down()
        {
        }
    }
}
