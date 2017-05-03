﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompSpyWeb.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
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