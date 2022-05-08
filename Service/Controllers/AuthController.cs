using D_A_L.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Service.Controllers
{
    public class AuthController : ApiController
    {
      

        // POST: api/Auth
        public Admin Post(Admin a)
        {
            using (Context cn = new Context())
            {

                var admin = cn.Admins.Single(x => x.Password == a.Password && x.Username == a.Username);
               
                if (admin != null)
                {

                    return admin;
                }

                return null;

            }

        }
          

     
    }
}
