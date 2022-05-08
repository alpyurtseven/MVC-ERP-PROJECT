namespace D_A_L.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<D_A_L.Model.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
     
            ContextKey = "DAL.Model.Context";
        }

        protected override void Seed(D_A_L.Model.Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
