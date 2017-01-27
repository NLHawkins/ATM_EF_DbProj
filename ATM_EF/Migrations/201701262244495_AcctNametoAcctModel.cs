namespace ATM_EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AcctNametoAcctModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accts", "AcctName", c => c.String());
            DropColumn("dbo.Accts", "NameId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accts", "NameId", c => c.String());
            DropColumn("dbo.Accts", "AcctName");
        }
    }
}
