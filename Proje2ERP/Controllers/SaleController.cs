using D_A_L.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Proje2ERP.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        // GET: Sale
        public ActionResult Index()
        {
            using (Context cn = new Context())
            {
                

                return View(cn.Invoices.Include("Staffs").Include("Clients").ToList());
            }
            
        }

        [HttpGet]
        public ActionResult NewSale()
        {

          
                using (Context cn = new Context())
                {
                    List<SelectListItem> cat = (from x in cn.Products.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = x.ProductName + " " +x.Color,
                                                    Value = x.ProductId.ToString()
                                                }
                                               ).ToList();
                ViewBag.catval = cat;
            }
              
                return View();
            
          
        }

        [HttpPost]
        public ActionResult NewSale(FormCollection saleData)
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddInvoice()
        {
            using (Context cn = new Context())
            {

                List<SelectListItem> val2 = (from x in cn.Clients.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = x.ClientName,
                                                 Value = x.ClientId.ToString()
                                             }
                    ).ToList();
                ViewBag.val2 = val2;
                return View(new Invoice());
            }

        }

        [HttpPost]
        public async Task<ActionResult> AddInvoice(Invoice d)
        {
            DateTime dt = DateTime.Now;
            d.StaffId = 1;
            d.Date = dt;
            d.TotalPrice = 0;
            using (HttpClient cl = new HttpClient())
            {
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Invoice/AddInvoice/";
                var model = JsonConvert.SerializeObject(d);
                var content = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await cl.PostAsync(uri, content);
                var invoiceResponse = await response.Content.ReadAsStringAsync();
                var deserializedInvoice = JsonConvert.DeserializeObject<Invoice>(invoiceResponse);
                TempData["a"] = deserializedInvoice.InvoiceId;
                Logging.LogInfo(Session["Username"].ToString() + " " + DateTime.Now + " " + deserializedInvoice.InvoiceId + " numaralı fişi ekledi");
                return RedirectToAction("NewSale", d.InvoiceId);
            }
          
          
        }

    }
}