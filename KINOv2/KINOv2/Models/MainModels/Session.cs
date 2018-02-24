using KINOv2.Models.ReferenceBooks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.MainModels
{
    public delegate bool seatFindDelegate(Seat s);
    public class Session
    {
        [Key]
        public int LINK { get; set; }
        //Фильм 
        public virtual Film Film { get; set; }
        [Display(Name = "Фильм")]
        [Required]
        public int? FilmLINK { get; set; }
        //Время
        [Display(Name = "Дата и время")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime SessionTime { get; set; }
        //Зал
        public virtual Hall Hall { get; set; }
        [Display(Name = "Зал")]
        [Required]
        public int? HallLINK { get; set; }

        public IEnumerable<Seat> Seats { get; set; }

        //Стоимость билета
        [Display(Name = "Стоимость билета")]
        [Required]
        public int Cost { get; set; }
        //Флаг актуальности
        public bool? Archived { get; set; }
    }

    public class SessionComparer : Comparer<Session>
    {
        public override int Compare(Session x, Session y)
        {
            return x.SessionTime.CompareTo(y.SessionTime);
        }
    }
    
}
