using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Notlarim101.WebApp.Data
{
    public class Notlarim101WebAppContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public Notlarim101WebAppContext() : base("name=Notlarim101WebAppContext")
        {
        }

        public System.Data.Entity.DbSet<Notlarim101.Entity.NotlarimUser> NotlarimUsers { get; set; }

        public System.Data.Entity.DbSet<Notlarim101.Entity.Note> Notes { get; set; }

        public System.Data.Entity.DbSet<Notlarim101.Entity.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<Notlarim101.Entity.Comment> Comments { get; set; }
    }
}