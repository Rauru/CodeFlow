using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Codeflow.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace Codeflow.Dal
{
    public class CodeFlowContext : DbContext
    {
        public CodeFlowContext() : base("CodeFlowContext")
        {
        }
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserQuestionVotes> Qvotes { get; set; }
        public DbSet<UserQuestionMinus> Qvotesminus { get; set; }
        public DbSet<Upvotes> AUpvotes { get; set; } 
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}