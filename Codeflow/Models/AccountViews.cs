using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codeflow.Models
{
    public class AccountViews
    {
        [Key]
        public Guid Id { get; set; }

        public Guid QuestionId { get; set; }
    }
}