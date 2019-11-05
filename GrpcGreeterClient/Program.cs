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

            //Verify all inputs
            Console.Write("RFW ID: ");
            string rfw = Console.ReadLine();

            Console.Write("Benchmark Type (DVD ot NDB): ");
            string benchmark = Console.ReadLine();

            Console.Write("Workload Metric \n(CPU = 1, NetIn = 2, NetOut = 3, Mem = 4): ");
            string metric = Console.ReadLine();

            Console.Write("Batch Unit: ");
            string batchUnit = Console.ReadLine();

            Console.Write("Batch ID: ");
            string batchId = Console.ReadLine();

            Console.Write("Batch Size: ");
            string batchSize = Console.ReadLine();

            var reply = await client.GetWorkloadAsync(new WorkloadRequest
            {
                Rfw = rfw,
                BenchMark = (WorkloadRequest.Types.BenchMarkType)Enum.Parse(typeof(WorkloadRequest.Types.BenchMarkType), benchmark),
                Metric = (WorkloadRequest.Types.WorkloadType)Enum.Parse(typeof(WorkloadRequest.Types.WorkloadType), metric),
                BatchUnit = Int32.Parse(batchUnit),
                BatchId = Int32.Parse(batchId),
                BatchSize = Int32.Parse(batchSize)
            }) ;
            

            Console.WriteLine("Greeting: " + reply);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
