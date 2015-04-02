using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codeflow.Models
{
    public class Answer
    {
        [Key]
        public Guid Id { get; set; }

        public string Answerstring { get; set; }
        public int Votes { get; set; }
        public string OwnerName { get; set; }
        public Guid QuestionID { get; set; }
        public Guid AccountID { get; set; }

        public virtual Question Question { get; set; }
        //public virtual Account Account { get; set; }
        public virtual ICollection<Upvotes> Upvotes{ get; set; }
    }
}