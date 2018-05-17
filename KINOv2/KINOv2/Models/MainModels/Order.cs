using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KINOv2.Models.MainModels
{
    public class Order
    {
        [Key]
        public int LINK { get; set; }
        // Стоимость заказа
        public int Cost { get; set; }
        //Ключ подтверждения
        public string ValidationKey { get; set; }
        //Дата оформления
        public DateTime? Date { get; set; }
        // Ссылка на клиента 
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public IEnumerable<Seat> Seats { get; set; }
        public bool Paid { get; set; }
        public string PayStatus { get; set; }
    }
}
