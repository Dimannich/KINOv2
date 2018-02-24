using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.ReferenceBooks
{
    public class Genre
    {
        [Key]
        public int LINK { get; set; }
        //Название жанра
        public string Name { get; set; }
    }
}
