using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.ReferenceBooks
{
    public class QA
    {
        [Key]
        public int LINK { get; set; }
        [Display (Name = "Вопрос")]
        public string Name { get; set; }
        [Display(Name = "Ответ")]
        public string Content { get; set; }
    }
}
