﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace breezenetcore21.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Tag cannot be longer than 50 characters.")]
        public string name { get; set; }
    }
}
