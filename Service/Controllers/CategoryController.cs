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
    public class CategoryController : ApiController
    {

        public List<Category> Get()
        {
            using (Context cn = new Context())
            {
                return cn.Categories.ToList();
            }
        }

        // GET: api/Client/5
        public Category Get(int id)
        {
            using (Context cn = new Context())
            {
                return cn.Categories.Single(x => x.CategoryId == id);
            }
        }

        // POST: api/Client
        public void Post(Category c)
        {
            using (Context cn = new Context())
            {
                cn.Categories.Add(c);
                cn.SaveChanges();
            }

        }

        // PUT: api/Client/5
        public void Put(int id, Category c)
        {
            using (Context cn = new Context())
            {
                var category = cn.Categories.Find(c.CategoryId);
                category.CategoryName = c.CategoryName;
                cn.SaveChanges();
            }
        }

        // DELETE: api/Client/5
        public void Delete(int id)
        {
        }
    }
}
