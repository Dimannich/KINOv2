using KINOv2.Models.ReferenceBooks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.MainModels
{
    public class UserRequest
    {
        [Key]
        public int LINK { get; set; }
        //Дата формирования обращения
        public DateTime Date { get; set; }
        //Ссылка на юзера
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        //Email
        public string Email { get; set; }
        //Текст обращения
        [Display(Name = "Текст обращения")]
        public string Content { get; set; }
        //Тема обращения
        [Display(Name = "Тема обращения")]
        public UserRequestSubject UserRequestSubject { get; set; }
        public int? UserRequestSubjectLINK { get; set; }
    }
}
