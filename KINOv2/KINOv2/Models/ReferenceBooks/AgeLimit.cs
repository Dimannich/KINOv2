using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.ReferenceBooks
{
    public class AgeLimit
    {
        [Key]
        public int LINK { get; set; }
        //Значение для сравнения
        public int Amout { get; set; }
        //Значение для вывода 
        public string Value { get; set; }
    }
}
