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
    
    public class StaffController : Controller
    {
        // GET: Staff
        public ActionResult Index()
        {
           
            if (Session["Authority"].ToString() != "1") return RedirectToAction("Index","Home");
            List<Staff> model;
            using (HttpClient cl = new HttpClient())
            {
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                var response = cl.GetAsync("https://localhost:44372/api/Staff/Get");
                if (response.Result.IsSuccessStatusCode) model = JsonConvert.DeserializeObject<List<Staff>>(response.Result.Content.ReadAsStringAsync().Result);
                else model = new List<Staff>();
                return View(model);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetStaff(int id)
        {
            if (Session["Authority"].ToString() != "1") return RedirectToAction("Index", "Home");
            using (HttpClient cl = new HttpClient())
            {
                Staff model;
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Staff/Get/" + id;
                var response = cl.GetAsync(uri);
                if (response.Result.IsSuccessStatusCode) model = JsonConvert.DeserializeObject<Staff>(response.Result.Content.ReadAsStringAsync().Result);
                else model = new Staff();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateStaff(Staff c)
        {
            if (Session["Authority"].ToString() != "1") return RedirectToAction("Index", "Home");
            using (HttpClient cl = new HttpClient())
            {
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Staff/Put/" + c.StaffId;
                var model = JsonConvert.SerializeObject(c);
                var content = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await cl.PutAsync(uri, content);
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public ActionResult AddStaff()
        {
            if (Session["Authority"].ToString() != "1") return RedirectToAction("Index", "Home");
            return View(new Staff());
        }
        [HttpPost]
        public async Task<ActionResult> AddStaff(Staff c)
        {
            if (Session["Authority"].ToString() != "1") return RedirectToAction("Index", "Home");
            using (HttpClient cl = new HttpClient())
            {
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Staff/Post";
                var model = JsonConvert.SerializeObject(c);
                var content = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await cl.PostAsync(uri, content);
                return RedirectToAction("Index");
            }

        }
    }
}