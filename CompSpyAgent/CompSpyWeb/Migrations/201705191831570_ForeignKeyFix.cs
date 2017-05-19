namespace CompSpyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Classroom", "Creator_UserID", "dbo.User");
            DropForeignKey("dbo.Computer", "Creator_UserID", "dbo.User");
            DropIndex("dbo.Classroom", new[] { "Creator_UserID" });
            DropIndex("dbo.Computer", new[] { "Creator_UserID" });
            DropColumn("dbo.Classroom", "CreatorID");
            DropColumn("dbo.Classroom", "EditorID");
            DropColumn("dbo.Computer", "CreatorID");
            DropColumn("dbo.Computer", "EditorID");
            RenameColumn(table: "dbo.Classroom", name: "Creator_UserID", newName: "CreatorID");
            RenameColumn(table: "dbo.Classroom", name: "Editor_UserID", newName: "EditorID");
            RenameColumn(table: "dbo.Computer", name: "Creator_UserID", newName: "CreatorID");
            RenameColumn(table: "dbo.Computer", name: "Editor_UserID", newName: "EditorID");
            RenameIndex(table: "dbo.Classroom", name: "IX_Editor_UserID", newName: "IX_EditorID");
            RenameIndex(table: "dbo.Computer", name: "IX_Editor_UserID", newName: "IX_EditorID");
            AlterColumn("dbo.Classroom", "CreatorID", c => c.Int(nullable: false));
            AlterColumn("dbo.Computer", "CreatorID", c => c.Int(nullable: false));
            CreateIndex("dbo.Classroom", "CreatorID");
            CreateIndex("dbo.Computer", "CreatorID");
            AddForeignKey("dbo.Classroom", "CreatorID", "dbo.User", "UserID", cascadeDelete: false);
            AddForeignKey("dbo.Computer", "CreatorID", "dbo.User", "UserID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Computer", "CreatorID", "dbo.User");
            DropForeignKey("dbo.Classroom", "CreatorID", "dbo.User");
            DropIndex("dbo.Computer", new[] { "CreatorID" });
            DropIndex("dbo.Classroom", new[] { "CreatorID" });
            AlterColumn("dbo.Computer", "CreatorID", c => c.Int());
            AlterColumn("dbo.Classroom", "CreatorID", c => c.Int());
            RenameIndex(table: "dbo.Computer", name: "IX_EditorID", newName: "IX_Editor_UserID");
            RenameIndex(table: "dbo.Classroom", name: "IX_EditorID", newName: "IX_Editor_UserID");
            RenameColumn(table: "dbo.Computer", name: "EditorID", newName: "Editor_UserID");
            RenameColumn(table: "dbo.Computer", name: "CreatorID", newName: "Creator_UserID");
            RenameColumn(table: "dbo.Classroom", name: "EditorID", newName: "Editor_UserID");
            RenameColumn(table: "dbo.Classroom", name: "CreatorID", newName: "Creator_UserID");
            AddColumn("dbo.Computer", "EditorID", c => c.Int());
            AddColumn("dbo.Computer", "CreatorID", c => c.Int(nullable: false));
            AddColumn("dbo.Classroom", "EditorID", c => c.Int());
            AddColumn("dbo.Classroom", "CreatorID", c => c.Int(nullable: false));
            CreateIndex("dbo.Computer", "Creator_UserID");
            CreateIndex("dbo.Classroom", "Creator_UserID");
            AddForeignKey("dbo.Computer", "Creator_UserID", "dbo.User", "UserID");
            AddForeignKey("dbo.Classroom", "Creator_UserID", "dbo.User", "UserID");
        }
    }
}
