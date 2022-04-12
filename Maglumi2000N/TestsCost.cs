using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maglumi2000N
{
    [Table("TestsCost")]
    public class TestsCost
    {
        public long Id { get; set; }
        public int TestId { get; set; }
        public long PatientId { get; set; }
    }
}
