using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codeflow.Models
{
    public class Answer
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Answer")]
        [StringLength(100, ErrorMessage = "The password must contain at least 50 characters", MinimumLength = 50)]
        public string Answerstring { get; set; }

        public DateTime ATime { get; set; }

        public int Votes { get; set; }
        public string OwnerName { get; set; }
        public Guid QuestionID { get; set; }
        public Guid AccountID { get; set; }

        public virtual Question Question { get; set; }
        //public virtual Account Account { get; set; }
        public virtual ICollection<Upvotes> Upvotes{ get; set; }
        public virtual ICollection<Downvotes> Downvotes { get; set;}
    }
}