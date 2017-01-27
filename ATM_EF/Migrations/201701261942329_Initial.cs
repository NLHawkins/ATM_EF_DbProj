namespace ATM_EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameId = c.String(),
                        CurrentBalance = c.Double(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ATM_Log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                        Adjustment = c.String(),
                        LogBalance = c.Double(nullable: false),
                        Acct_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accts", t => t.Acct_Id)
                .Index(t => t.Acct_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ATM_Log", "Acct_Id", "dbo.Accts");
            DropForeignKey("dbo.Accts", "User_Id", "dbo.Users");
            DropIndex("dbo.ATM_Log", new[] { "Acct_Id" });
            DropIndex("dbo.Accts", new[] { "User_Id" });
            DropTable("dbo.ATM_Log");
            DropTable("dbo.Users");
            DropTable("dbo.Accts");
        }
    }
}
