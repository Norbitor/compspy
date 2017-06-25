namespace CompSpyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAbuserToComputer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Abuse", "AbuserID", "dbo.User");
            AddForeignKey("dbo.Abuse", "AbuserID", "dbo.Computer");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Abuse", "AbuserID", "dbo.Computer");
            AddForeignKey("dbo.Abuse", "AbuserID", "dbo.User");
        }
    }
}
