using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Maglumi2000N
{
    public class MAHDbContext : DbContext
    {
        public DbSet<Patient>Patients{get;set;}
        public DbSet<TestsCost> TestsCosts { get; set; }
    }
}
