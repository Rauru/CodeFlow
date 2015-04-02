using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codeflow.Models
{
    public class UserQuestionVotes
    {
        [Key]
        public Guid QvoteId { get; set; }

        public Guid QuestionId { get; set; }

        public Guid AccountId { get; set; }
    }
}