using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.MainModels
{
    public class Seat
    {
        [Key]
        public int LINK { get; set; }
        //Ряд
        public int Row { get; set; }
        //Номер
        public int Number { get; set; }
        //Статус брони
        public bool IsBooked { get; set; }
        //Ссылка на заказ
        public virtual Order Order { get; set; }
        public int? OrderLINK { get; set; }
        // Сеанс
        public virtual Session Session { get; set; }
        public int? SessionLINK { get; set; }
    }
}
