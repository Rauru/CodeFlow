using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Codeflow.Models;

namespace Codeflow.Dal
{
    public class CodeFlowInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<CodeFlowContext>
    {
        protected override void Seed(CodeFlowContext context)
        //using (var db = new BlogContext())
        {

            Guid idt = Guid.NewGuid();
            Guid idt2 = Guid.NewGuid();
            Guid idtq = Guid.NewGuid();
            Guid idtq2 = Guid.NewGuid();
            Guid idta = Guid.NewGuid();
            Guid idta2 = Guid.NewGuid();
            var accounts = new List<Account>
            {
                new Account{ID = idt, FirstName = "Raul" ,LastName = "Lopez" ,CreationdDateTime = DateTime.Parse("2005-09-01") , Email = "raul@gmail.com", Password = "abc12345", ConfirmPassword = "abc12345"},
                new Account{ID = idt2, FirstName = "Rauru", LastName = "Lopez", CreationdDateTime = DateTime.Parse("2005-09-01"), Email = "Rauru@gmail.com", Password = "ab123456", ConfirmPassword = "ab123456"}
                
            };

            accounts.ForEach(s => context.Accounts.Add(s));
            
            context.SaveChanges();

            var questions = new List<Question>
            {
                new Question
                {
                    QuestionID = idtq,
                    QuestionString = "what is love?",
                    AccountID = idt,
                    Votes = 1
                },
                new Question
                {
                    QuestionID = idtq2,
                    QuestionString = "what is the meaning of life",
                    AccountID = idt2,
                    Votes = 2
                }

            };
            questions.ForEach(s=> context.Questions.Add(s));
            context.SaveChanges();

            var UserQuestionVotes = new List<UserQuestionVotes>
            {
                new UserQuestionVotes
                {
                    QvoteId = Guid.NewGuid(),
                    AccountId = idt2,
                    QuestionId = idtq2
                }

            };
            UserQuestionVotes.ForEach(s=> context.Qvotes.Add(s));
            context.SaveChanges();

            var UserQuestionMinus = new List<UserQuestionMinus>
            {
                new UserQuestionMinus
                {AccountId = idt,
                    QuestionId = idtq,
                    QvoteId = Guid.NewGuid()
                    
                }
            };

            UserQuestionMinus.ForEach(s=> context.Qvotesminus.Add(s));
            context.SaveChanges();

            var answers = new List<Answer>
            {
                new Answer
                {
                    Id = idta,
                    Answerstring = "baby dont hurt me", 
                    Votes = 0,
                    OwnerName = "Raul",
                    QuestionID = idtq,
                    AccountID = idt
                    
                },
                new Answer
                {
                    Id = idta2,
                    Answerstring = "42",
                    Votes = 1,
                    OwnerName = "Rauru",
                    QuestionID = idtq2,
                    AccountID = idt2
                    
                }

            };
            answers.ForEach(s=> context.Answers.Add(s));
            context.SaveChanges();
        }
    }
}