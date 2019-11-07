using System;
using System.Net.Http;
using System.Threading.Tasks;
using GrpcWorload;
using Grpc.Net.Client;

namespace GrpcGreeterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            var channel = GrpcChannel.ForAddress("https://localhost:5001/gRPC");
            var client = new Workload.WorkloadClient(channel);

            while (true)
            {
                //Verify all inputs
                string rfw = "";
                while (true)
                {
                    Console.Write("RFW ID: ");
                    rfw = Console.ReadLine();
                    if (!String.IsNullOrEmpty(rfw))
                        break;
                    Console.WriteLine("RFW ID is empty !");
                }


                //validate benchmarktype
                string benchmark = "";
                while (true)
                {
                    Console.Write("Benchmark Type (DVDTrain = 0, DVDTest = 1, NDBenchTrain = 2, NDBench = 3): ");
                    benchmark = Console.ReadLine();
                    if (benchmark == "0" || benchmark == "1" || benchmark == "2" || benchmark == "3")
                        break;
                    Console.WriteLine("Invalid Benchmark Type!");
                }

                //validate metric type
                string metric = "";
                while (true)
                {
                    Console.Write("Workload Metric (CPU = 0, NetworkIn = 1, NetworkOut = 2, Memory = 3, FinalTarget = 4): ");
                    metric = Console.ReadLine();
                    if (metric == "0" || metric == "1" || metric == "2" || metric == "3" || metric == "4")
                        break;
                    Console.WriteLine("Invalid Worload Metric!");
                }

                //validate batch unit is a number
                int batchUnit = 0;
                while (true)
                {
                    Console.Write("Batch Unit (integer): ");
                    string str = Console.ReadLine();
                    if (int.TryParse(str, out batchUnit))
                        break;
                    Console.WriteLine("Not a valid interger!");
                }

                //validate batch id is a number
                int batchId = 0;
                while (true)
                {
                    Console.Write("Batch ID (integer): ");
                    string str = Console.ReadLine();
                    if (int.TryParse(str, out batchId))
                        break;
                    Console.WriteLine("Not a valid interger!");
                }

                //validate batch size is a number
                int batchSize = 0;
                while (true)
                {
                    Console.Write("Batch Size (integer): ");
                    string str = Console.ReadLine();
                    if (int.TryParse(str, out batchSize))
                        break;
                    Console.WriteLine("Not a valid interger!");
                }

                //send request then get resonse in "reply" variable
                var reply = await client.GetWorkloadAsync(new WorkloadRequest
                {
                    Rfw = rfw,
                    BenchMark = (WorkloadRequest.Types.BenchMarkType)Enum.Parse(typeof(WorkloadRequest.Types.BenchMarkType), benchmark),
                    Metric = (WorkloadRequest.Types.MetricType)Enum.Parse(typeof(WorkloadRequest.Types.MetricType), metric),
                    BatchUnit = batchUnit,
                    BatchId = batchId,
                    BatchSize = batchSize
                });

                //output reply
                Console.WriteLine(" \n\n");
                Console.WriteLine("Server Response: \nRFWID: " + reply.Rfw + "\nLastBatchID: " + reply.LastBatchId + "\nBatches: " + reply.Batches);
                Console.WriteLine("Press any key to continue or type 'quit' to exit...");

                //exit loop
                string quit = Console.ReadLine();
                if (quit == "quit")
                    break;

            }
        }
    }
}
