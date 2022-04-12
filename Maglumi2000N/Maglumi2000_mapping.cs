using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maglumi2000N
{
    [Table("Maglumi2000_mapping")]
    public class Maglumi2000_mapping
    {
        public int Id { get; set; }
        public string ParameterName { get; set; }
        public int ReportDefId { get; set; }
        public int TestId { get; set; }
    }
}
