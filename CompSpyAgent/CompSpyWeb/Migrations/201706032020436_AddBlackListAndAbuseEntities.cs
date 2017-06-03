namespace CompSpyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBlackListAndAbuseEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abuse",
                c => new
                    {
                        AbuseID = c.Int(nullable: false, identity: true),
                        AbuserID = c.Int(nullable: false),
                        ScreenPath = c.String(),
                        DetectedOn = c.DateTime(nullable: false),
                        Read = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AbuseID)
                .ForeignKey("dbo.User", t => t.AbuserID, cascadeDelete: true)
                .Index(t => t.AbuserID);
            
            CreateTable(
                "dbo.Blacklist",
                c => new
                    {
                        BlacklistID = c.Int(nullable: false, identity: true),
                        ProcessName = c.String(),
                        ClassroomID = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatorID = c.Int(nullable: false),
                        LastEdit = c.DateTime(),
                        EditorID = c.Int(),
                    })
                .PrimaryKey(t => t.BlacklistID)
                .ForeignKey("dbo.Classroom", t => t.ClassroomID)
                .ForeignKey("dbo.User", t => t.CreatorID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.EditorID)
                .Index(t => t.ClassroomID)
                .Index(t => t.CreatorID)
                .Index(t => t.EditorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blacklist", "EditorID", "dbo.User");
            DropForeignKey("dbo.Blacklist", "CreatorID", "dbo.User");
            DropForeignKey("dbo.Blacklist", "ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.Abuse", "AbuserID", "dbo.User");
            DropIndex("dbo.Blacklist", new[] { "EditorID" });
            DropIndex("dbo.Blacklist", new[] { "CreatorID" });
            DropIndex("dbo.Blacklist", new[] { "ClassroomID" });
            DropIndex("dbo.Abuse", new[] { "AbuserID" });
            DropTable("dbo.Blacklist");
            DropTable("dbo.Abuse");
        }
    }
}
