using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompSpyWeb.Models
{
    public class Computer
    {
        public int ID { get; set; }
        public int ClassroomID { get; set; }
        public string IPAddress { get; set; }
        public string StationDiscriminant { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatorID { get; set; }
        public DateTime? lastEdit { get; set; }
        public int? EditorID { get; set; }

        public virtual Classroom Classroom { get; set; }
        public virtual User Creator { get; set; }
        public virtual User Editor { get; set; }
    }
}
