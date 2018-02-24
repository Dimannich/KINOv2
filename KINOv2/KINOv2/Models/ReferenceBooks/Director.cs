using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.ReferenceBooks
{
    public class Director
    {
        [Key]
        public int LINK { get; set; }
        //Имя 
        public string Name { get; set; }
        //Фамилия
        public string Surname { get; set; }
    }
}
