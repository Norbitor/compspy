using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CompSpyWeb.Models
{
    public class Classroom
    {
        public int ID { get; set; }

        [DisplayName("Nazwa")]
        public string Name { get; set; }

        [DisplayName("Lokalizacja")]
        public string Location { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatorID { get; set; }

        public DateTime? LastEdit { get; set; }

        public int? EditorID { get; set; }

        [ForeignKey("CreatorID")]
        public virtual User Creator { get; set; }

        [ForeignKey("EditorID")]
        public virtual User Editor { get; set; }
    }
}
