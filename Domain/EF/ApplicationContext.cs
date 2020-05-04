using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

    }
}
