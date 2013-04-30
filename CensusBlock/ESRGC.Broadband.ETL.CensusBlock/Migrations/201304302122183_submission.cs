namespace ESRGC.Broadband.ETL.CensusBlock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class submission : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Submission", "SubmssionTimeStarted", c => c.DateTime(nullable: false));
            AddColumn("dbo.Submission", "SubmissionTimeCompleted", c => c.DateTime(nullable: false));
            AddColumn("dbo.Submission", "LastStatusUpdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Submission", "RecordsStored", c => c.Int());
            AddColumn("dbo.Submission", "DataCount", c => c.Int(nullable: false));
            AddColumn("dbo.Submission", "ProgressPercentage", c => c.Int(nullable: false));
            AlterColumn("dbo.Submission", "Status", c => c.String(maxLength: 50));
            DropColumn("dbo.Submission", "SubmissionTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Submission", "SubmissionTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Submission", "Status", c => c.String(maxLength: 30));
            DropColumn("dbo.Submission", "ProgressPercentage");
            DropColumn("dbo.Submission", "DataCount");
            DropColumn("dbo.Submission", "RecordsStored");
            DropColumn("dbo.Submission", "LastStatusUpdate");
            DropColumn("dbo.Submission", "SubmissionTimeCompleted");
            DropColumn("dbo.Submission", "SubmssionTimeStarted");
        }
    }
}
