using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CompSpyWeb.ViewModels
{
    public class ClassroomViewModel
    {
        public int ID { get; set; }

        [DisplayName("Nazwa")]
        public string Name { get; set; }

        [DisplayName("Lokalizacja")]
        public string Location { get; set; }

        [DisplayName("Liczba komputerów")]
        public int ComputersCount { get; set; }

        [DisplayName("Aktywne komputery")]
        public int ActiveComputers { get; set; }
    }
}