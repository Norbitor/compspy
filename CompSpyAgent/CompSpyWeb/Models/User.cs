using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CompSpyWeb.Models
{
    public class User
    {
        public int UserID { get; set; }

        [DisplayName("Login")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string Login { get; set; }

        [DisplayName("Hasło")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Imię")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string FirstName { get; set; }

        [DisplayName("Nazwisko")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string LastName { get; set; }

        [DisplayName("Administrator")]
        public bool IsAdmin { get; set; }

        [DisplayName("Zablokowany")]
        public bool IsLocked { get; set; }

        [DisplayName("Ostatnie logowanie")]
        public DateTime? LastLogin { get; set; }

        public int LoginAttempts { get; set; }

        [DisplayName("Data utworzenia")]
        public DateTime CreatedOn { get; set; }

        public int? CreatorID { get; set; }

        [DisplayName("Data ostatniej edycji")]
        public DateTime? LastEdit { get; set; }

        public int? EditorID { get; set; }

        public virtual User Creator { get; set; }
        public virtual User Editor { get; set; }
    }
}
