using FluentMigrator;

namespace Cookbook.Infrastructure.Migrations.Versions;

[Migration(version: 1, description: "Create table to save user's informations")]
public class Version001 : VersionBase
{
    public override void Up()
    {
        CreateTable("Users")
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable();
    }
}