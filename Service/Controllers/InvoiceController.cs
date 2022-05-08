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
    public class InvoiceController : ApiController
    {

        [HttpPost]
        public Invoice AddInvoice(Invoice i)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            else
            {
                using (Context cn = new Context())
                {
                    DateTime dt = DateTime.Now;
                    i.Date = dt;
                    i.StaffId = 1;
                    i.TotalPrice = 0;

                    cn.Invoices.Add(i);
                    cn.SaveChanges();
                    return i;
                }
            }
            
        }
    }
}
