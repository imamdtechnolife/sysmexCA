using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;

namespace Maglumi2000N
{
   public class AppDbContext : DbContext
    {
        public DbSet<PatientRecord> PatientRecords { get; set; }

        public DbSet<ResultRecord> Resultrecords { get; set; }
        public DbSet<Maglumi2000_mapping> Maglumi2000_Mappings { get; set; }
    }
}
