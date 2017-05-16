namespace CompSpyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplayNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "IsLocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "IsLocked");
        }
    }
}
