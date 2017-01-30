namespace ATM_EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resetTablesacctInfoUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ATM_Log", "Acct_Id", "dbo.Accts");
            DropForeignKey("dbo.ATM_Log", "User_Id", "dbo.Users");
            DropIndex("dbo.ATM_Log", new[] { "Acct_Id" });
            DropIndex("dbo.ATM_Log", new[] { "User_Id" });
            RenameColumn(table: "dbo.Accts", name: "User_Id", newName: "AcctUser_Id");
            RenameIndex(table: "dbo.Accts", name: "IX_User_Id", newName: "IX_AcctUser_Id");
            DropColumn("dbo.ATM_Log", "Acct_Id");
            DropColumn("dbo.ATM_Log", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ATM_Log", "User_Id", c => c.Int());
            AddColumn("dbo.ATM_Log", "Acct_Id", c => c.Int());
            RenameIndex(table: "dbo.Accts", name: "IX_AcctUser_Id", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Accts", name: "AcctUser_Id", newName: "User_Id");
            CreateIndex("dbo.ATM_Log", "User_Id");
            CreateIndex("dbo.ATM_Log", "Acct_Id");
            AddForeignKey("dbo.ATM_Log", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.ATM_Log", "Acct_Id", "dbo.Accts", "Id");
        }
    }
}
