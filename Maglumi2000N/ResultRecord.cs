using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maglumi2000N
{
    public class ResultRecord
    {
        [Key]
        public long ResultRecordId { get; set; }
        [ForeignKey("PatientRecord")]

        public long PatientRecordId { get; set; }

        public string Category { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string LongName { get; set; }

        public string Value { get; set; }

        public string Unit { get; set; }

        public string Range { get; set; }

        public DateTime? ReportDate { get; set; }

        public PatientRecord PatientRecord { get; set; }
    }
}
