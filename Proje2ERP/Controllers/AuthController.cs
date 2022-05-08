using D_A_L.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Proje2ERP.Controllers
{
    
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }
        public static Token token;
        [HttpPost]
        public async Task<ActionResult> Login(Admin a)
        {
            string baseAddress = "https://localhost:44372";
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", a.Username},
                   {"password", a.Password},
               };
                var tokenResponse = await client.PostAsync(baseAddress + "/token", new FormUrlEncodedContent(form));
                //var token = tokenResponse.Content.ReadAsStringAsync().Result;  
                token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
            }
            using (HttpClient cl = new HttpClient())
            {
                string uri = "https://localhost:44372/api/Auth/Post";
                var model = JsonConvert.SerializeObject(a);
                var content = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await cl.PostAsync(uri, content);
                var admin = JsonConvert.DeserializeObject<Admin>(response.Content.ReadAsStringAsync().Result);
                if (response.StatusCode != System.Net.HttpStatusCode.OK) return RedirectToAction("Index", "Auth");
     
                else
                {
                    FormsAuthentication.SetAuthCookie(a.Username, false);
                    Session["Username"] = admin.Username;
                    Session["Authority"] = admin.Authority;
                   
                    return RedirectToAction("Index", "Client");
                }



            }

        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Auth");
        }
    }
}