using D_A_L.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Proje2ERP.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       public class IndexViewModel
        {
            public float TotalEarning { get; set; }
            public int ClientCount { get; set; }
            public int ProductCount { get; set; }
            public int SaleCount { get; set; }
        }

        public ActionResult Index()
        {

            IndexViewModel ivm = new IndexViewModel();
            using (Context cn = new Context())
            {
                ivm.TotalEarning = (float)cn.Invoices.Sum(x => x.TotalPrice);
                ivm.ClientCount = cn.Clients.Count();
                ivm.ProductCount = cn.Products.Count();
                ivm.SaleCount = cn.Invoices.Count();
            }
            Logging.LogInfo("Anasayfa Açıldı");
                return View(ivm);
        }
    }

}