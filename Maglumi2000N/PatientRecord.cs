using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maglumi2000N
{
    public class PatientRecord
    {
        [Key]
        public long PatientRecordId { get; set; }

        public string PatientId { get; set; }

        public int? SequenceId { get; set; }

        public string InstrumentName { get; set; }

        public DateTime? ReportDate { get; set; }
    }
}
