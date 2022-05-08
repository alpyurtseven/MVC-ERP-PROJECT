using D_A_L.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proje2ERP.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        // GET: Product
        public async Task<ActionResult> Index()
        {
            using (HttpClient cl = new HttpClient())
            {
                if(AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
             
                var response = await cl.GetAsync("https://localhost:44372/api/Product/Get");
                List<Product> model;
                if (response.StatusCode==System.Net.HttpStatusCode.OK)
                {
                     model = JsonConvert.DeserializeObject<List<Product>>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                     model = new List<Product>();
                }
              
                return View(model);
            }
        }

        public ActionResult Get(int id)
        {
            using (HttpClient cl = new HttpClient())
            {
                Product model;
                using (Context cn = new Context()) {
                    List<SelectListItem> cat = (from x in cn.Categories.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = x.CategoryName,
                                                    Value = x.CategoryId.ToString()
                                                }
                                         ).ToList();
                    ViewBag.catval = cat;
                }

                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                var response = cl.GetAsync("https://localhost:44372/api/Product/Get/" + id);
                if (response.Result.IsSuccessStatusCode) model = JsonConvert.DeserializeObject<Product>(response.Result.Content.ReadAsStringAsync().Result);
                else model = new Product();               
                return View(model);
            }
        }

      

        [HttpPost]
        public async Task<ActionResult> UpdateProduct(Product p)
        {
            using (HttpClient cl = new HttpClient())
            {
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Product/Put/" + p.ProductId;
                var model = JsonConvert.SerializeObject(p);
                var content = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await cl.PutAsync(uri, content);
                Logging.LogInfo(Session["Username"].ToString() + " " + DateTime.Now + " " + p.ProductCode + " numaralı ürünü düzenledi");
                return RedirectToAction("Index");
            }
        }

      

        [HttpGet]
        public ActionResult AddProduct()
        {
            using (Context cn = new Context())
            {
                List<SelectListItem> cat = (from x in cn.Categories.ToList()
                                            select new SelectListItem
                                            {
                                                Text = x.CategoryName,
                                                Value = x.CategoryId.ToString()
                                            }
                                     ).ToList();
                ViewBag.catval = cat;
            }
            return View(new Product());
        }
        
     


        [HttpPost]
        public async Task<ActionResult> AddProduct(Product p)
        {
            using (HttpClient cl = new HttpClient())
            {
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Product/Post";
                var model = JsonConvert.SerializeObject(p);
                var content = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await cl.PostAsync(uri, content);
                Logging.LogInfo(Session["Username"].ToString() + " " + DateTime.Now + " " + p.ProductCode + " numaralı ürünü ekledi");
                return RedirectToAction("Index");
            }

        }
     


    }
}