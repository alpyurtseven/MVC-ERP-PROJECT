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
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
         
            using (HttpClient cl = new HttpClient())
            {
                List<Category> model;
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                var response = cl.GetAsync("https://localhost:44372/api/Category/Get");
                if (response.Result.IsSuccessStatusCode) model = JsonConvert.DeserializeObject<List<Category>>(response.Result.Content.ReadAsStringAsync().Result);
                else model = new List<Category>();
                return View(model);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetCategory(int id)
        {
            using (HttpClient cl = new HttpClient())
            {
                Category model;
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Category/Get/" + id;
                var response = cl.GetAsync(uri);
                if (response.Result.IsSuccessStatusCode) model = JsonConvert.DeserializeObject<Category>(response.Result.Content.ReadAsStringAsync().Result);
                else model = new Category();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCategory(Category c)
        {
            using (HttpClient cl = new HttpClient())
            {
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Category/Put/" + c.CategoryId;
                var model = JsonConvert.SerializeObject(c);
                var content = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await cl.PutAsync(uri, content);
                Logging.LogInfo(Session["Username"].ToString() + " " + DateTime.Now + " " + c.CategoryName + " adlı kategoriyi düzenledi");
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View(new Category());
        }
        [HttpPost]
        public async Task<ActionResult> AddCategory(Category c)
        {
            using (HttpClient cl = new HttpClient())
            {
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Category/Post";
                var model = JsonConvert.SerializeObject(c);
                var content = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await cl.PostAsync(uri, content);
                Logging.LogInfo(Session["Username"].ToString() + " " + DateTime.Now + " " + c.CategoryName + " adlı kategoriyi ekledi");
                return RedirectToAction("Index");
            }

        }
    }
}