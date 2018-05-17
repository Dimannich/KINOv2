using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using KINOv2.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KINOv2.Controllers.Kassa
{
    public class KassaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KassaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public string Result(string OutSum, int InvId, string SignatureValue)
        {
            string hashBae = $"{OutSum}:{InvId}:{KassaOptions.Pass2}";
            // build CRC value
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(hashBae));

            StringBuilder sbSignature = new StringBuilder();
            foreach (byte b in bSignature)
                sbSignature.AppendFormat("{0:x2}", b);

            string sCrc = sbSignature.ToString();   
            if (sCrc.ToLowerInvariant() != SignatureValue.ToLowerInvariant())
                return "NOT_OK";

            var order = _context.Orders.FirstOrDefault(o => o.LINK == InvId);
            if (order == null)
                return "NOT_OK";
            order.Paid = true;
            order.PayStatus = "Оплачено";
            _context.SaveChanges();
            return "OK" + InvId;
        }
        public IActionResult Success(double OutSum, int InvId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.LINK == InvId);
            ViewBag.ValidationKey = order.ValidationKey;
            return View();
        }
        public IActionResult Fail(double OutSum, int InvId, string Culture)
        {
            var order = _context.Orders.FirstOrDefault(o => o.LINK == InvId);
            order.Paid = false;
            order.PayStatus = "Оплата не прошла";
            return View();
        }
    }
}