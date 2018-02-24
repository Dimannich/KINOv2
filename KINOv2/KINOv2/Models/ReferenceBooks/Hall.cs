using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.ReferenceBooks
{
    public class Hall
    {
        [Key]
        public int LINK { get; set; }
        //Название зала
        public string Name { get; set; }
        //Изображение
        public string Image { get; set; }
        //Количество мест
        public int SeatsNumber { get; set; }
    }
}
