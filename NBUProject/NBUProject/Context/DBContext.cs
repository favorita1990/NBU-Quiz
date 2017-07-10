using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using NBUProject.Models;

namespace NBUProject.Contex
{
   
    public class DBContext : IdentityDbContext<UserModel>
    {
        public DBContext()
            : base("NBUProjectConnection")
        {
        }

        public static DBContext Create()
        {
            return new DBContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poll>().ToTable("Polls");
            modelBuilder.Entity<PollQuastion>().ToTable("PollQuastions");
            modelBuilder.Entity<PollAnswer>().ToTable("PollAnswers");
            modelBuilder.Entity<User>().ToTable("UsersFromThePoll");
            modelBuilder.Entity<UserPoll>().ToTable("UserPolls");
            modelBuilder.Entity<UserPollQuestion>().ToTable("UserPollQuestions");
            modelBuilder.Entity<UserPollAnswer>().ToTable("UserPollAnswers");
            modelBuilder.Entity<SentEmail>().ToTable("SentEmails");

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Poll> Polls { get; set; }
        public virtual DbSet<PollQuastion> PollQuastions { get; set; }
        public virtual DbSet<PollAnswer> PollAnswers { get; set; }
        public virtual DbSet<User> UsersFromThePoll { get; set; }
        public virtual DbSet<UserPoll> UserPolls { get; set; }
        public virtual DbSet<UserPollQuestion> UserPollQuestions { get; set; }
        public virtual DbSet<UserPollAnswer> UserPollAnswers { get; set; }
        public virtual DbSet<SentEmail> SentEmails { get; set; }

    }
}