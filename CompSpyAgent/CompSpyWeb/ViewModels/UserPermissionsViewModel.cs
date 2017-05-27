using CompSpyWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CompSpyWeb.ViewModels
{
    public class UserPermissionsViewModel
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
        
        public ICollection<Classroom> ClassroomsWithPermissions { get; set; }

        public ICollection<Classroom> AllClassrooms { get; set; }
    }
}