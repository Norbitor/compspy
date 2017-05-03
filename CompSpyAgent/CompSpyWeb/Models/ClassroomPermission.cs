using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CompSpyWeb.Models
{
    public class ClassroomPermission
    {
        [Key]
        public int ClassroomPermissionID { get; set; }
        public int ClassroomID { get; set; }
        public int UserID { get; set; }

        public virtual Classroom Classroom { get; set; }
        public virtual User User { get; set; }
    }
}
