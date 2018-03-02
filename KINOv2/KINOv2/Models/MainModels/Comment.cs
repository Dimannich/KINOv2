using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.MainModels
{
    public class Comment
    {
        public Comment()
        {
            Rating = new List<Rating>();
        }

        [Key]
        public int LINK { get; set; }
        //Текст комментария
        public string Text { get; set; }
        //Ссылка на фильм
        public Film Film { get; set; }
        public int? FilmLINK { get; set; }
        //Ссылка на пользователя 
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        //Ссылка на другой коммент
        public Comment BaseComment { get; set; }
        public int? BaseCommentLINK { get; set; }
        //Оценки
        public ICollection<Rating> Rating { get; set; }
    }
}
