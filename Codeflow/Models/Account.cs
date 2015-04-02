using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Codeflow.Models
{
    //[Table("studentdetails")]
    public class Account
    {
        [Key]
        public Guid ID { get; set; }
        [DisplayName("User")]
        [Required]
        [StringLength(50, ErrorMessage = "The name must contain 2 to 50 character", MinimumLength = 2)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        [StringLength(50, ErrorMessage = "The last name must contain 2 to 50 character", MinimumLength = 2)]
        public string LastName { get; set; }
        public DateTime CreationdDateTime { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Email { get; set; }

        public bool Verifiedmail { get; set; }


        [Required]
        [StringLength(16, ErrorMessage = "The password must contain 8 to 16 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [StringLength(16, ErrorMessage = "The password must contain 8 to 16 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<AccountViews> AccountViews { get; set; }
    }
}