using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CompSpyWeb.Models
{
    public class Computer
    {
        public int ID { get; set; }

        public int ClassroomID { get; set; }

        [DisplayName("Adres IP")]
        public string IPAddress { get; set; }

        [DisplayName("Oznaczenie komputera")]
        public string StationDiscriminant { get; set; }

        public bool IsConnected { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatorID { get; set; }

        public DateTime? lastEdit { get; set; }

        public int? EditorID { get; set; }

        public virtual Classroom Classroom { get; set; }

        public virtual User Creator { get; set; }

        public virtual User Editor { get; set; }
    }
}
