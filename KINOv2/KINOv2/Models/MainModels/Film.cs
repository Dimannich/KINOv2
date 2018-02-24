using KINOv2.Models.ReferenceBooks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.MainModels
{
    public class Film
    {
        [Key]
        public int LINK { get; set; }
        //Название 
        [Display(Name = "Название")]
        [Required]
        public string Name { get; set; }
        //Постер
        [Display(Name = "Название файла-постера")]
        [Required]
        public string Poster { get; set; }
        //Год выпуска
        [Display(Name = "Год выхода на экран")]
        [Required]
        public int ReleaseYear { get; set; }
        //Страна
        public virtual Country Country { get; set; }
        [Display(Name = "Страна производитель")]
        public int? CountryLINK { get; set; }
        //Жанр
        public virtual Genre Genre { get; set; }
        [Display(Name = "Жанр")]
        public int? GenreLINK { get; set; }
        //Режиссер
        public virtual Director Director { get; set; }
        [Display(Name = "Режиссер")]
        public int? DirectorLINK { get; set; }
        //Продолжительность
        [Display(Name = "Длительность (в минутах)")]
        [Required]
        public string Duration { get; set; }
        //Возрастное ограничение 
        public virtual AgeLimit AgeLimit { get; set; }
        [Display(Name = "Возрастное ограничение")]
        public int? AgeLimitLINK { get; set; }
        //Флаг актуальности
        public bool? Archived { get; set; }
    }
}
