using KINOv2.Models.MainModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.ContentManageViewModels
{
    public class FilmManageViewModel
    {
        public Film Film { get; set; }
        public Session Session { get; set; }

        [Display(Name = "Файл изображения")]
        public IFormFile UploadedFile { get; set; }
    }
}
