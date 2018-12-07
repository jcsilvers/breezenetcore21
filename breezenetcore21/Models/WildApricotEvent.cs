﻿using System;
using System.Collections.Generic;
using System.Text;

namespace breezenetcore21.Models
{
    public class WildApricotEvent
    {
        public int Id { get; set; }
        public int WaId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public ICollection<WildApricotRegistrationType> WaRegistrationTypes { get; set; }
        

    }
}
