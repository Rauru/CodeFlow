using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codeflow.Models
{
    public class Question
    {
        [Key]
        public Guid QuestionID { get; set; }

        public string QuestionTittle { get; set; }
        public string QuestionString { get; set; }
        public Guid AccountID { get; set; }
        public int Votes{ get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<UserQuestionVotes> Votetotals { get; set; }
        public virtual ICollection<UserQuestionMinus> Voteminus { get; set; } 
        
    }
}