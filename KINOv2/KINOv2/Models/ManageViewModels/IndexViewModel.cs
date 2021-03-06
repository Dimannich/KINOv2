﻿using KINOv2.Models.MainModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }
        
        [Display(Name = "Возраст")]
        public int? Age { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string SurName { get; set; }

        [Display(Name = "О себе")]
        [DataType(DataType.MultilineText)]
        public string About { get; set; }

        public List<Film> Films { get; set; }

        public string ProfileImage { get; set; }

        public string StatusMessage { get; set; }
        
        [Display(Name = "Отображать избранные фильмы")]
        public bool SelectedFilmsVisible { get; set; }

        [Display(Name = "Отображать личную информацию")]
        public bool PersonalInfoVisible { get; set; }

        [Display(Name = "Изображение")]
        public IFormFile ProfileUploaded { get; set; }
    }
}
