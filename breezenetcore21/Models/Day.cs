using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace breezenetcore21.Models
{
    public class Day
    {
        public int Id { get; set; }

        [Required]
        public DateTime AgendaDay { get; set; }
        public List<Section> Sections { get; set; }
    }
}
