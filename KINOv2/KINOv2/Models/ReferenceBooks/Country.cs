using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.ReferenceBooks
{
    public class Country
    {
        [Key]
        public int LINK { get; set; }
        //Называние страны
        public string Name { get; set; }
    }
}
