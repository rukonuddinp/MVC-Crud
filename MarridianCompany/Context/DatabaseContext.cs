using MarridianCompany.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MarridianCompany.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(): base("MarridianCompanyDB")
        {

        }


        public DbSet<FoodGroup> FoodGroups { get; set; }
        public DbSet<FoodDetail> FoodDetails { get; set; }
    }
}