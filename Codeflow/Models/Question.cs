using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codeflow.Models
{
    public class Question
    {
        [Key]
        public Guid QuestionID { get; set; }

        [Required]
        [DisplayName("Title")]
        [StringLength(100, ErrorMessage = "The password must contain at least 50 characters", MinimumLength = 50)]
        public string QuestionTittle { get; set; }

        [Required]
        [DisplayName("Description")]
        public string QuestionString { get; set; }

        public DateTime QTime { get; set; }

        public Guid AccountID { get; set; }
        public int Votes{ get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<UserQuestionVotes> Votetotals { get; set; }
        public virtual ICollection<UserQuestionMinus> Voteminus { get; set; }
        public virtual ICollection<QuestionViews> Questionviews { get; set; }
        
    }
}