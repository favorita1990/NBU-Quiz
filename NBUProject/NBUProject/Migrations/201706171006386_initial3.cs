namespace NBUProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserPollAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Answer = c.String(),
                        PollQuastionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserPollQuestions", t => t.PollQuastionId, cascadeDelete: true)
                .Index(t => t.PollQuastionId);
            
            CreateTable(
                "dbo.UserPollQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quastion = c.String(),
                        PollQuastionId = c.Int(nullable: false),
                        PollId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserPolls", t => t.PollId, cascadeDelete: true)
                .Index(t => t.PollId);
            
            CreateTable(
                "dbo.UserPolls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TimeBegins = c.DateTime(nullable: false),
                        TimeEnd = c.DateTime(nullable: false),
                        PollId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UsersFromThePoll", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UsersFromThePoll",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        FacultyNumber = c.String(),
                        IPAddress = c.String(),
                        HashCode = c.String(),
                        PollId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Polls", t => t.PollId, cascadeDelete: true)
                .Index(t => t.PollId);
            
            AddColumn("dbo.PollQuastions", "UserPoll_Id", c => c.Int());
            CreateIndex("dbo.PollQuastions", "UserPoll_Id");
            AddForeignKey("dbo.PollQuastions", "UserPoll_Id", "dbo.UserPolls", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPollAnswers", "PollQuastionId", "dbo.UserPollQuestions");
            DropForeignKey("dbo.UserPollQuestions", "PollId", "dbo.UserPolls");
            DropForeignKey("dbo.UserPolls", "UserId", "dbo.UsersFromThePoll");
            DropForeignKey("dbo.UsersFromThePoll", "PollId", "dbo.Polls");
            DropForeignKey("dbo.PollQuastions", "UserPoll_Id", "dbo.UserPolls");
            DropIndex("dbo.UsersFromThePoll", new[] { "PollId" });
            DropIndex("dbo.UserPolls", new[] { "UserId" });
            DropIndex("dbo.UserPollQuestions", new[] { "PollId" });
            DropIndex("dbo.UserPollAnswers", new[] { "PollQuastionId" });
            DropIndex("dbo.PollQuastions", new[] { "UserPoll_Id" });
            DropColumn("dbo.PollQuastions", "UserPoll_Id");
            DropTable("dbo.UsersFromThePoll");
            DropTable("dbo.UserPolls");
            DropTable("dbo.UserPollQuestions");
            DropTable("dbo.UserPollAnswers");
        }
    }
}
