using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KINOv2.Models.MainModels;
using Microsoft.AspNetCore.Identity;

namespace KINOv2.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
            SelectedFilms = new List<Film>();
        }

        //Изображение в профиле
        public string ProfileImage { get; set; }
        //Возраст
        public int? Age { get; set; }
        //Город
        public string City { get; set; }
        //Имя 
        public string Name { get; set; }
        //Фамилия
        public string SurName { get; set; }
        //Избранные фильмы
        public ICollection<Film> SelectedFilms { get; set; }
    }
}
