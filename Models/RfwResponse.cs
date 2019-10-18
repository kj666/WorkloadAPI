using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workload.Models
{
    public class RfwResponse
    {
        public string RfwID { get; set; }
        
        public long LastBatchId { get; set; }

        public List<Workload> Samples { get; set; }
    }
}
