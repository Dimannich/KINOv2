using KINOv2.Models.AdditionalEFEntities;
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
        public Film()
        {
            Rating = new List<Rating>();
            FilmUsers = new List<FilmUser>();
        }

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
        [Display(Name = "Год выпуска")]
        [Required]
        public int ReleaseYear { get; set; }
        //Страна
        public Country Country { get; set; }
        [Display(Name = "Страна")]
        public int? CountryLINK { get; set; }
        //Жанр
        public virtual Genre Genre { get; set; }
        [Display(Name = "Жанр")]
        public int? GenreLINK { get; set; }
        //Режиссер
        public Director Director { get; set; }
        [Display(Name = "Режиссер")]
        public int? DirectorLINK { get; set; }
        //Продолжительность
        [Display(Name = "Длительность")]
        [Required]
        public string Duration { get; set; }
        //Возрастное ограничение 
        public AgeLimit AgeLimit { get; set; }
        [Display(Name = "Возрастное ограничение")]
        public int? AgeLimitLINK { get; set; }
        //Флаг актуальности
        public bool? Archived { get; set; }
        //Описание фильма
        [Display(Name = "Описание")]
        public string Description { get; set; }
        //Ссылка на трейлер
        [Display(Name = "Трейлер")]
        [Url]
        public string TrailerLink { get; set; }
        //Глобальный рейтинг (КП)
        [Display(Name = "Рейтинг Кинопоиска")]
        public int? GlobalRating { get; set; }
        //Локальный рейтинг
        [Display(Name = "Оценка пользователей")]
        public int? LocalRating { get; set; }
        //Оценки фильма
        public ICollection<Rating> Rating { get; set; }
        //Пользователи, добавившие фильм в избранное
        public ICollection<FilmUser> FilmUsers { get; set; }
        // Комментарии к фильму
        public ICollection<Comment> Comments { get; set; }
        // Сеансы фильма
        public ICollection<Session> Sessions { get; set; }
        //Наличие в главном слайдере
        public bool InMainSlider { get; set; }
        //Постер для главного слайдера
        public string MainSliderPoster { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
