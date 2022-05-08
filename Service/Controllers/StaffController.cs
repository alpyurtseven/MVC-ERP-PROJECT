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
    public class StaffController : ApiController
    {
        public List<Staff> Get()
        {
            using (Context cn = new Context())
            {
                return cn.Staffs.ToList();
            }
        }

        // GET: api/Client/5
        public Staff Get(int id)
        {
            using (Context cn = new Context())
            {
                return cn.Staffs.Single(x => x.StaffId == id);
            }
        }

        // POST: api/Client
        public void Post(Staff s)
        {
            using (Context cn = new Context())
            {
                cn.Staffs.Add(s);
                cn.SaveChanges();
            }
        }

        // PUT: api/Client/5
        public void Put(int id, Staff s)
        {
            using (Context cn = new Context())
            {
                var staff = cn.Staffs.Find(s.StaffId);
                staff.StaffName = s.StaffName;
                staff.StaffSurname = s.StaffSurname;
                staff.Status = s.Status;
                cn.SaveChanges();
            }
        }
    }
}
