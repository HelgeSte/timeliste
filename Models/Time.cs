using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace timeliste.Models {
	public class Time {
        public int Id { get; set; }
        
        public string AnsattNr { get; set; }

        public int ProsjektId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Slutt { get; set; }

        public double Timer { get; set; } 

        [StringLength(500)]
        [DisplayName("Kommentar")]
        public string Kommentar { get; set; }

    }
}