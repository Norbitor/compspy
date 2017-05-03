using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompSpyWeb.Models
{
    public class Classroom
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatorID { get; set; }
        public DateTime? LastEdit { get; set; }
        public int? EditorID { get; set; }

        public virtual User Creator { get; set; }
        public virtual User Editor { get; set; }
    }
}
