using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using System;
namespace GSM.Models{
    public class Info{
        public Invoice Invoice { get; set; }
        public Category Category { get; set; }
    }
}