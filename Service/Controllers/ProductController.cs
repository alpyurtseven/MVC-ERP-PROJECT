using D_A_L.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace Service.Controllers
{

    public class ProductController : ApiController
    {
        // GET: api/Product
        public List<Product> Get()
        {
            using (Context cn = new Context())
            {
                var prodocut = cn.Products.Include("Category").ToList();
                return prodocut;
            }
        }
       

        public Product Get(int id)
        {
            using (Context cn = new Context())
            {
                
                return cn.Products.Include("Category").Single(x => x.ProductId == id);
            }
        }
      

        public void Post(Product p)
        {
            using (Context cn = new Context())
            {
     
                p.Tmt = p.Stock * p.Mt;
                if (p.CategoryId == 6)
                {
                    p.mt2 = (p.Width / 1000 * p.Height / 1000);
                    p.Tmt = p.mt2 * p.Stock;

                }
                p.Kg = p.Mkg * p.Tmt;
                p.mt2 = p.Height * p.Width;
                cn.Products.Add(p);
                cn.SaveChanges();
            }

        }
       

        // PUT: api/Product/5
        public void Put(int id, Product p)
        {
            using (Context cn = new Context())
            {
                var prod = cn.Products.Find(p.ProductId);
                prod.Status = p.Status;
                prod.Stock = p.Stock;
                prod.Pprice = p.Pprice;
                prod.Sprice = p.Sprice;
                prod.CategoryId = p.CategoryId;
                prod.Image = p.Image;
                prod.Mkg = p.Mkg;
                prod.Mt = p.Mt;
                prod.ProductCode = p.ProductCode;
                prod.CriticalPoint = p.CriticalPoint;
                prod.Width = p.Width;
                prod.Height = p.Height;
                prod.Color = p.Color;
                if (p.CategoryId != 5)
                {
                    p.Kg = (p.Mt * p.Stock * p.Mkg);

                }
                else
                {
                    p.Kg = p.Stock * 15;
                }
                if (p.CategoryId != 5 && p.CategoryId != 6)
                {
                    p.Tmt = p.Mt * p.Stock;
                }
                if (p.CategoryId == 6)
                {
                    p.mt2 = (p.Width / 1000 * p.Height / 1000);
                    p.Tmt = p.mt2 * p.Stock;

                }
                prod.Kg = p.Kg;
                prod.Tmt = p.Tmt;
                prod.mt2 = p.mt2;
                cn.SaveChanges();
            }
        }
       
    }
}
