using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Controllers.ApiControllers
{
    public class ProfileSerializer
    {
        public string Username { get; set; }
        //Изображение в профиле
        public string ProfileImage { get; set; }
        //Возраст
        public int? Age { get; set; }
        //Email
        public string Email { get; set; }
        //Город
        public string City { get; set; }
        //Имя 
        public string Name { get; set; }
        //Фамилия
        public string SurName { get; set; }
        //О себе
        public string About { get; set; }
        //Отображение избранных фильмов остальным юзерам
        public bool? SelectedFilmsVisible { get; set; }
        //Отображение персональной информации остальным юзерам
        public bool? PersonalInfoVisible { get; set; }
    }

    public class FilmSerializer
    {
        public int LINK { get; set; }
        public string Name { get; set; }
        public string Poster { get; set; }
        public int ReleaseYear { get; set; }
        public string Country { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Duration { get; set; }
        public string AgeLimit { get; set; }
        public bool? Archived { get; set; }
        public string Description { get; set; }
        public string TrailerLink { get; set; }
        public int? GlobalRating { get; set; }
        public int? LocalRating { get; set; }
        public int AmountComment { get; set; }
    }
    public class SessionSerializer
    {
        public int LINK { get; set; }
        public string Film { get; set; }
        public int FilmLINK { get; set; }
        public DateTime SessionTime { get; set; }
        public string Hall { get; set; }
        public IEnumerable<SeatSerializer> Seats { get; set; }
        public int Cost { get; set; }
        public string Poster { get; set; }
        public bool? Archived { get; set; }
        public string Duration { get; set; }
    }
    public class SeatSerializer
    {
        public int LINK { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public bool IsBooked { get; set; }
    }
    public class CommentSerializer
    {
        public int LINK { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime? Date { get; set; }
        public int? BaseCommentLINK { get; set; }
        public int FilmLINK { get; set; }
        public UserSerializer User { get; set; }
        public int? YourRate { get; set; }
    }
    public class UserSerializer
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
    }
    public class OrderSerializer
    {
        public int LINK { get; set; }
        // Стоимость заказа
        public int Cost { get; set; }
        //Ключ подтверждения
        public string ValidationKey { get; set; }
        //Дата оформления
        public DateTime? Date { get; set; }

        public string Username { get; set; }
        public string Hall { get; set; }
        public string FilmName { get; set; }
        public int FilmLINK { get; set; }
        public IEnumerable<SeatSerializer> Seats { get; set; }
    }
    public class HallSerializer
    {
        public int LINK { get; set; }
        public string Hall { get; set; }
        public IEnumerable<SessionSerializer> sessions {get;set;}
    }
    public class DaySerializer
    {
        public DateTime date { get; set; }
        public IEnumerable<HallSerializer> halls { get; set; }
    }
}
