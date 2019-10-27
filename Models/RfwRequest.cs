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
        public BenchMarkType BenchmarkType { get; set; }

        /// <summary>
        /// Type of workload Metric (CPU or NetworkIn or NetworkOut or Memory)
        /// </summary>
        public WorkloadType WorloadMetric { get; set; }

        public Int32 BatchUnit { get; set; }

        public Int32 BatchId { get; set; }

        public Int32 BatchSize { get; set; }

    }

    public enum BenchMarkType
    {
        DVDTrain = 0,
        DVDTest = 1,
        NDBenchTrain = 2,
        NDBenchTest = 3
    };

    public enum WorkloadType
    {
        CPU = 0,
        NetworkIn = 1,
        NetworkOut = 2,
        Memory = 3
    }

    public class RfwResponse
    {
        public string RfwID { get; set; }

        public long LastBatchId { get; set; }

        public List<Batch> Batches { get; set; }
    }

    public class Workload
    {
        public Workload() { }

        public int LineID { get; set; }

        public double CPUUtilization_Average { get; set; }

        public double NetworkIn_Average { get; set; }

        public double NetworkOut_Average { get; set; }

        public double MemoryUtilization_Average { get; set; }

    }

    public class Batch
    {
        public int Id { get; set; }

        public List<Workload> workloads { get; set; }
    }
}
