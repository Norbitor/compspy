namespace CompSpyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComputerConnectionMark : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Computer", "IsConnected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Computer", "IsConnected");
        }
    }
}
