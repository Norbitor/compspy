using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CompSpyWeb.Models
{
    public class User
    {
        public int UserID { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string Login { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? LastLogin { get; set; }
        public int LoginAttempts { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatorID { get; set; }
        public DateTime? LastEdit { get; set; }
        public int? EditorID { get; set; }

        public virtual User Creator { get; set; }
        public virtual User Editor { get; set; }
    }
}
