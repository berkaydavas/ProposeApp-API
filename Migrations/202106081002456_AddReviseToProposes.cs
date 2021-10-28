namespace ProposeAppAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReviseToProposes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proposes", "reviseNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proposes", "reviseNumber");
        }
    }
}
