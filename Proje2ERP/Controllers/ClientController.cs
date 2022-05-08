using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using D_A_L.Model;
using Newtonsoft;
using Newtonsoft.Json;


namespace Proje2ERP.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        // GET: Client
        public async Task<ActionResult> Index()
        {
            using (HttpClient cl = new HttpClient())
            {
                List<Client> model;
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                
                var response = cl.GetAsync("https://localhost:44372/api/Client/Get");
                if (response.Result.IsSuccessStatusCode) model = JsonConvert.DeserializeObject<List<Client>>(response.Result.Content.ReadAsStringAsync().Result);
                else model = new List<Client>();
                return View(model);
            }    
        }
        [HttpGet]
        public async Task<ActionResult> GetClient(int id)
        {
            using (HttpClient cl = new HttpClient())
            {
                Client model;
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Client/GetById/" + id;
                var response = cl.GetAsync(uri);
                if (response.Result.IsSuccessStatusCode) model = JsonConvert.DeserializeObject<Client>(response.Result.Content.ReadAsStringAsync().Result);
                else model = new Client();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateClient(Client c)
        {
            using (HttpClient cl = new HttpClient())
            {
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Client/Put/"+c.ClientId;
                var model = JsonConvert.SerializeObject(c);
                var content = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await cl.PutAsync(uri, content);
                Logging.LogInfo(Session["Username"].ToString() + " " + DateTime.Now + " " + c.ClientId + " numaralı müşteriyi düzenledi");
                return RedirectToAction("Index");
            }
      
        }

       [HttpGet]
        public ActionResult AddClient()
        {
            return View(new Client());
        }
        [HttpPost]
        public async Task<ActionResult> AddClient(Client c)
        {
            using (HttpClient cl = new HttpClient())
            {
                if (AuthController.token != null)
                {
                    cl.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("bearer", AuthController.token.AccessToken);
                }
                string uri = "https://localhost:44372/api/Client/Post";
                var model = JsonConvert.SerializeObject(c);
                var content = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await cl.PostAsync(uri, content);
                Logging.LogInfo(Session["Username"].ToString() + " " + DateTime.Now + " " + c.ClientName + c.ClientSurname + " adlı müşteriyi ekledi");
                return RedirectToAction("Index");
            }

        }



    }
}