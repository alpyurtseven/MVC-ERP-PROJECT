using D_A_L.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Service.Controllers
{
    [Authorize]
    public class ClientController : ApiController
    {
        // GET: api/Client
        public List<Client> Get()
        {
            using (Context cn = new Context())
            {
                return cn.Clients.ToList();
            }
        }

        // GET: api/Client/5
        public Client GetById(int id)
        {
            using (Context cn = new Context())
            {
                return cn.Clients.Single(x => x.ClientId == id);
            }
        }

        // POST: api/Client
        public void Post(Client c)
        {
            using (Context cn = new Context())
            {     
                cn.Clients.Add(c);
                cn.SaveChanges();
            }

        }

        // PUT: api/Client/5
        public void Put(int id, Client c)
        {
            using (Context cn = new Context())
            {
                var client = cn.Clients.Find(c.ClientId);
                client.ClientName = c.ClientName;
                client.ClientSurname = c.ClientSurname;
                client.PhoneNumber = c.PhoneNumber;
                client.PhoneNumber1 = c.PhoneNumber1;
                client.PhoneNumber2 = c.PhoneNumber2;
                client.ClientMail = c.ClientMail;
                client.ClientAddress = c.ClientAddress;
                client.Status = c.Status;
                cn.SaveChanges();
            }
        }

        // DELETE: api/Client/5
        public void Delete(int id)
        {
        }
    }
}
