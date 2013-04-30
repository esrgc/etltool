namespace ESRGC.Broadband.ETL.CensusBlock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamedSubmissionTimeStarted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Submission", "SubmissionTimeStarted", c => c.DateTime());
            DropColumn("dbo.Submission", "SubmssionTimeStarted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Submission", "SubmssionTimeStarted", c => c.DateTime());
            DropColumn("dbo.Submission", "SubmissionTimeStarted");
        }
    }
}
