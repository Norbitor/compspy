namespace CompSpyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectIdFieldsAndConnectionId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClassroomPermission", "ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.Computer", "ClassroomID", "dbo.Classroom");
            DropPrimaryKey("dbo.Classroom");
            DropPrimaryKey("dbo.Computer");
            DropColumn("dbo.Classroom", "ID");
            DropColumn("dbo.Computer", "ID");
            DropColumn("dbo.Computer", "IsConnected");
            AddColumn("dbo.Classroom", "ClassroomID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Computer", "ComputerID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Computer", "ConnectionID", c => c.String());
            AddPrimaryKey("dbo.Classroom", "ClassroomID");
            AddPrimaryKey("dbo.Computer", "ComputerID");
            AddForeignKey("dbo.ClassroomPermission", "ClassroomID", "dbo.Classroom", "ClassroomID", cascadeDelete: true);
            AddForeignKey("dbo.Computer", "ClassroomID", "dbo.Classroom", "ClassroomID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Computer", "ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.ClassroomPermission", "ClassroomID", "dbo.Classroom");
            DropPrimaryKey("dbo.Computer");
            DropPrimaryKey("dbo.Classroom");
            DropColumn("dbo.Computer", "ConnectionID");
            DropColumn("dbo.Computer", "ComputerID");
            DropColumn("dbo.Classroom", "ClassroomID");
            AddColumn("dbo.Computer", "IsConnected", c => c.Boolean(nullable: false));
            AddColumn("dbo.Computer", "ID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Classroom", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Computer", "ID");
            AddPrimaryKey("dbo.Classroom", "ID");
            AddForeignKey("dbo.Computer", "ClassroomID", "dbo.Classroom", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ClassroomPermission", "ClassroomID", "dbo.Classroom", "ID", cascadeDelete: true);
        }
    }
}
