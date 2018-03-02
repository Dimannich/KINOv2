using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.MainModels
{
    public class Rating
    {
        [Key]
        public int LINK { get; set; }
        //Значение 
        public int Value { get; set; }
        //Пользователь
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
