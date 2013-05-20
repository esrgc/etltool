namespace ESRGC.Broadband.ETL.CensusBlock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        TicketID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IssueDate = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TicketID);
            
            AddColumn("dbo.Submission", "Ticket_TicketID", c => c.Int());
            AddForeignKey("dbo.Submission", "Ticket_TicketID", "dbo.Ticket", "TicketID");
            CreateIndex("dbo.Submission", "Ticket_TicketID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Submission", new[] { "Ticket_TicketID" });
            DropForeignKey("dbo.Submission", "Ticket_TicketID", "dbo.Ticket");
            DropColumn("dbo.Submission", "Ticket_TicketID");
            DropTable("dbo.Ticket");
        }
    }
}
