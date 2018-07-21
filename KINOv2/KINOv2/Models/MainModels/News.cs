using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.MainModels
{
    public class News
    {
        [Key]
        public int LINK { get; set; }
        //Дата публикации
        public DateTime PublishDate { get; set; }
        //Превью - картинка
        [Required]
        public string ImagePreview { get; set; }
        //Превью - тест
        [Required]
        public string TextPreview { get; set; }
        //Контент
        [Required]
        public string Content { get; set; }
        //Название
        [Required]
        public string Name { get; set; }
        public bool Archived { get; set; }
    }
}
