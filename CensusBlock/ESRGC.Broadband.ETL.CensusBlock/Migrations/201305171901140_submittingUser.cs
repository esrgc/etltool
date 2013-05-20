namespace ESRGC.Broadband.ETL.CensusBlock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class submittingUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Submission", "SubmittingUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Submission", "SubmittingUser");
        }
    }
}
