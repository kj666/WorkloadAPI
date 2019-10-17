using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workload.Models
{
    public class Workload
    {
        public Workload() { }

        public double CPUUtilization_Average { get; set; }

        public double NetworkIn_Average { get; set; }

        public double NetworkOut_Average { get; set; }

        public double MemoryUtilization_Average { get; set; }

    }
}
