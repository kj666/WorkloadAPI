
namespace Workload.Models
{
    public class RfwRequest
    {
        public string RfwID { get; set; }

        public BenchMarkType BenchmarkType { get; set; }

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
        Memory = 3,
        FinalTarget = 4
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

        public double FinalTarget { get; set; }

    }

    public class Batch
    {
        public int Id { get; set; }

        public List<double> values { get; set; }
    }

    public static class Data
    {
        public static List<Workload> DVDTrain = new List<Workload>();
        public static List<Workload> DVDTest = new List<Workload>();
        public static List<Workload> NDBTrain = new List<Workload>();
        public static List<Workload> NDBTest = new List<Workload>();

        public static string main_path = Path.Combine(Environment.CurrentDirectory) + "/Data";
        public static string path_DVD_Test = main_path + "/DVD-testing.csv";
        public static string path_DVD_Train = main_path + "/DVD-training.csv";
        public static string path_NDB_Test = main_path + "/NDBench-testing.csv";
        public static string path_NDB_Train = main_path + "/NDBench-training.csv";
    }
}
