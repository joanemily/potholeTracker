using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.Models
{
    public class Pothole
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public DateTime? DateReported { get; set; }
        public DateTime? DateInspected { get; set; }
        public DateTime? DateBeginRepair { get; set; }
        public DateTime? DateFinishRepair { get; set; }
        public string RepairStatus { get; set; }
        public int? Severity { get; set; } 
        public int? IsArchived { get; set; }
    }
}
