using KINOv2.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.AdditionalEFEntities
{
    public class FilmUser
    {
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public Film Film { get; set; }
        public int? FilmLINK { get; set; }
    }
}
