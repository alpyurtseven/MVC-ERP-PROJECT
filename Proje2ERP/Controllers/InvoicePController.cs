using D_A_L.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proje2ERP.Controllers
{
    public class InvoicePController : Controller
    {
        // GET: InvoiceP
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddP(FormCollection form)
        {
            using (Context cn= new Context())
            {
                Product p = new Product();
                string[] productId = form[0].Split(',');
                string[] quantity = form[1].Split(',');
                string[] unitPrice = form[2].Split(',');
                string[] totalPrice = form[3].Split(',');
                string invoiceId = form[4].Split(',')[0];
              
                List<InvoiceP> invp = new List<InvoiceP>();
                for (int i = 0; i < productId.Length; i++)
                {
                    InvoiceP ip = new InvoiceP();
                    ip.ProductId = int.Parse(productId[i]);
                    ip.Quantity = int.Parse(quantity[i]);
                    ip.UnitPrice = int.Parse(unitPrice[i]);
                    ip.InvoiceId = int.Parse(invoiceId);
                    ip.Price = int.Parse(totalPrice[i]);
                    var product = cn.Products.Find(ip.ProductId);
                    var invoice = cn.Invoices.Find(ip.InvoiceId);
                    product.Stock -= ip.Quantity;
                    invoice.TotalPrice = invoice.TotalPrice + ip.Price;
                    p = product;
                    invp.Add(ip);
                    UpdateKg(p);
                }
                cn.InvoicePs.AddRange(invp);
                cn.SaveChanges();
  
                return RedirectToAction("Index","Sale");

            }
                
        }


        void UpdateKg(Product p)
        {
            using (Context cn = new Context())
            {


                if (p.CategoryId != 5)
                {
                    p.Kg = (p.Mt * p.Stock * p.Mkg);

                }
                else
                {
                    p.Kg = p.Stock * 15;
                }
                if (p.CategoryId != 5 && p.CategoryId != 6)
                {
                    p.Tmt = p.Mt * p.Stock;
                }
                if (p.CategoryId == 6)
                {
                    p.mt2 = (p.Width / 1000 * p.Height / 1000);
                    p.Tmt = p.mt2 * p.Stock;

                }
                cn.SaveChanges();
            }
        }
    }
}