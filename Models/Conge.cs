using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryBeginners.Models
{
    public class Conge
    {
        [Key]
        public int CongeId { get; set; }

        public DateTime StartDay { get; set; }


        public DateTime EndDay { get; set; }



        public string type { get; set; }

        public string status { get; set; }

        public string employeeId { get; set; }
    }
}
