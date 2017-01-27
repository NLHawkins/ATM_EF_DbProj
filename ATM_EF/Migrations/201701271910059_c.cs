namespace ATM_EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class c : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ATM_Log", "AdjValue", c => c.Double(nullable: false));
            AddColumn("dbo.ATM_Log", "ActiveAcct_Id", c => c.Int());
            AddColumn("dbo.ATM_Log", "RecipientAcct_Id", c => c.Int());
            AddColumn("dbo.ATM_Log", "User_Id", c => c.Int());
            CreateIndex("dbo.ATM_Log", "ActiveAcct_Id");
            CreateIndex("dbo.ATM_Log", "RecipientAcct_Id");
            CreateIndex("dbo.ATM_Log", "User_Id");
            AddForeignKey("dbo.ATM_Log", "ActiveAcct_Id", "dbo.Accts", "Id");
            AddForeignKey("dbo.ATM_Log", "RecipientAcct_Id", "dbo.Accts", "Id");
            AddForeignKey("dbo.ATM_Log", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.ATM_Log", "LogId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ATM_Log", "LogId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ATM_Log", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ATM_Log", "RecipientAcct_Id", "dbo.Accts");
            DropForeignKey("dbo.ATM_Log", "ActiveAcct_Id", "dbo.Accts");
            DropIndex("dbo.ATM_Log", new[] { "User_Id" });
            DropIndex("dbo.ATM_Log", new[] { "RecipientAcct_Id" });
            DropIndex("dbo.ATM_Log", new[] { "ActiveAcct_Id" });
            DropColumn("dbo.ATM_Log", "User_Id");
            DropColumn("dbo.ATM_Log", "RecipientAcct_Id");
            DropColumn("dbo.ATM_Log", "ActiveAcct_Id");
            DropColumn("dbo.ATM_Log", "AdjValue");
        }
    }
}
