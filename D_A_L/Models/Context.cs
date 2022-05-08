
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace D_A_L.Model
{
    public class Context:DbContext
    {
        public Context()
            : base("name=Context"){
            Database.SetInitializer(new Configuration());
            Configuration.ProxyCreationEnabled = false;

        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }


        public DbSet<Client> Clients { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
    
        public DbSet<InvoiceP> InvoicePs { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Staff> Staffs { get; set; }
      

    }

   public class Configuration : CreateDatabaseIfNotExists<Context>
    {
        protected override void Seed(Context context)
        {
            
            context.Categories.Add(new Category() { CategoryName="Profil"});
            context.Clients.Add(new Client() { ClientName = "Alperen", ClientSurname = "Yurtseven", 
                ClientAddress = "-t", ClientMail = "alpyurtseveb99@gmail.com", PhoneNumber = "05078932701", 
                PhoneNumber1 = "05078932701", PhoneNumber2 = "05078932701", Status = true });
            context.SaveChanges();
        }

    }

}
