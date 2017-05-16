using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CompSpyWeb.ViewModels
{
    public class EditUserViewModel
    {
        public int UserID { get; set; }

        [DisplayName("Login")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string Login { get; set; }

        [DisplayName("Imię")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string FirstName { get; set; }

        [DisplayName("Nazwisko")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string LastName { get; set; }

        [DisplayName("Administrator")]
        public bool IsAdmin { get; set; }

        [DisplayName("Zmiana hasła")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Podane hasła nie zgadzają się")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}