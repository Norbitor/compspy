namespace CompSpyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassroomPermission",
                c => new
                    {
                        ClassroomPermissionID = c.Int(nullable: false, identity: true),
                        ClassroomID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClassroomPermissionID)
                .ForeignKey("dbo.Classroom", t => t.ClassroomID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ClassroomID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Classroom",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatorID = c.Int(nullable: false),
                        LastEdit = c.DateTime(),
                        EditorID = c.Int(),
                        Creator_UserID = c.Int(),
                        Editor_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.Creator_UserID)
                .ForeignKey("dbo.User", t => t.Editor_UserID)
                .Index(t => t.Creator_UserID)
                .Index(t => t.Editor_UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        LoginAttempts = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatorID = c.Int(),
                        LastEdit = c.DateTime(),
                        EditorID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.User", t => t.CreatorID)
                .ForeignKey("dbo.User", t => t.EditorID)
                .Index(t => t.CreatorID)
                .Index(t => t.EditorID);
            
            CreateTable(
                "dbo.Computer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClassroomID = c.Int(nullable: false),
                        IPAddress = c.String(),
                        StationDiscriminant = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatorID = c.Int(nullable: false),
                        lastEdit = c.DateTime(),
                        EditorID = c.Int(),
                        Creator_UserID = c.Int(),
                        Editor_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Classroom", t => t.ClassroomID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.Creator_UserID)
                .ForeignKey("dbo.User", t => t.Editor_UserID)
                .Index(t => t.ClassroomID)
                .Index(t => t.Creator_UserID)
                .Index(t => t.Editor_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Computer", "Editor_UserID", "dbo.User");
            DropForeignKey("dbo.Computer", "Creator_UserID", "dbo.User");
            DropForeignKey("dbo.Computer", "ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.ClassroomPermission", "UserID", "dbo.User");
            DropForeignKey("dbo.ClassroomPermission", "ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.Classroom", "Editor_UserID", "dbo.User");
            DropForeignKey("dbo.Classroom", "Creator_UserID", "dbo.User");
            DropForeignKey("dbo.User", "EditorID", "dbo.User");
            DropForeignKey("dbo.User", "CreatorID", "dbo.User");
            DropIndex("dbo.Computer", new[] { "Editor_UserID" });
            DropIndex("dbo.Computer", new[] { "Creator_UserID" });
            DropIndex("dbo.Computer", new[] { "ClassroomID" });
            DropIndex("dbo.User", new[] { "EditorID" });
            DropIndex("dbo.User", new[] { "CreatorID" });
            DropIndex("dbo.Classroom", new[] { "Editor_UserID" });
            DropIndex("dbo.Classroom", new[] { "Creator_UserID" });
            DropIndex("dbo.ClassroomPermission", new[] { "UserID" });
            DropIndex("dbo.ClassroomPermission", new[] { "ClassroomID" });
            DropTable("dbo.Computer");
            DropTable("dbo.User");
            DropTable("dbo.Classroom");
            DropTable("dbo.ClassroomPermission");
        }
    }
}
