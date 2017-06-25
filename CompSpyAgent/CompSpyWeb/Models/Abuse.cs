using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CompSpyWeb.Models
{
    public class Abuse
    {
        public int AbuseID { get; set; }

        public int AbuserID { get; set; }

        public string ScreenPath { get; set; }

        public DateTime DetectedOn { get; set; }

        public bool Read { get; set; }

        [ForeignKey("AbuserID")]
        public virtual Computer Abuser { get; set; }
    }
}