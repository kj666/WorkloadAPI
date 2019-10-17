using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workload.Models
{
    public class RfwRequest
    {
        public string RfwID { get; set; }

        /// <summary>
        /// DVD or NDBench
        /// </summary>
        public string BenchmarkType { get; set; }

        /// <summary>
        /// Type of workload Metric (CPU or NetworkIn or NetworkOut or Memory)
        /// </summary>
        public string WorloadMetric { get; set; }

        public long BatchUnit { get; set; }

        public long BatchId { get; set; }

        public long BatchSize { get; set; }

    }
}
