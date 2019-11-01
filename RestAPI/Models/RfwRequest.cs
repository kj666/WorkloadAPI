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

        public List<Workload> workloads { get; set; }
    }

    public static class Data
    {
        public static List<Workload> DVDTrain = new List<Workload>();
        public static List<Workload> DVDTest = new List<Workload>();
        public static List<Workload> NDBTrain = new List<Workload>();
        public static List<Workload> NDBTest = new List<Workload>();

        public static string main_path = "D:/Karthi/Document/Karthi/WorkloadAPI/RestAPI/Data";
        public static string path_DVD_Test = main_path + "/DVD-testing.csv";
        public static string path_DVD_Train = main_path + "/DVD-training.csv";
        public static string path_NDB_Test = main_path + "/NDBench-testing.csv";
        public static string path_NDB_Train = main_path + "/NDBench-training.csv";

        public static void PopulateList()
        {
            DVDTrain = ParseCsv(path_DVD_Train);
            DVDTest = ParseCsv(path_DVD_Test);
            NDBTrain = ParseCsv(path_NDB_Train);
            NDBTest = ParseCsv(path_NDB_Test);
        }

        public static List<Workload> ParseCsv(string path)
        {
            List<Workload> list = new List<Workload>();

            string[] lines = System.IO.File.ReadAllLines(path);

            for(int i = 1; i < lines.Count(); i++) { 
                string[] columns = lines[i].Split(',');

                Workload workload = new Workload();
                workload.LineID = i;
                for(int col = 0; col < columns.Count(); col++)
                {
                    if (col == 0)
                        workload.CPUUtilization_Average = double.Parse(columns[col]);
                    else if (col == 1)
                        workload.NetworkIn_Average = double.Parse(columns[col]);
                    else if (col == 2)
                        workload.NetworkOut_Average = double.Parse(columns[col]);
                    else if (col == 3)
                        workload.MemoryUtilization_Average = double.Parse(columns[col]);
                }
                list.Add(workload);
            }
            return list;
        }
    }
}
