using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codeflow.Models
{
    public class Downvotes
    {
        [Key]
        public Guid Id { get; set; }

        public Guid AnswerId { get; set; }

        public Guid AccountId { get; set; }
    }
}