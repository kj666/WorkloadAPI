using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicationServer.Models;
using Grpc.Core;
using Microsoft.Extensions.Logging;


namespace CommunicationServer
{
    public class WorkloadService : Workload.WorkloadBase
    {
        private readonly ILogger<WorkloadService> _logger;
        public WorkloadService(ILogger<WorkloadService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {

            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request
            });
        }

        public override Task<WorkloadResponse> GetWorkload(WorkloadRequest request, ServerCallContext context)
        {
            return Task.FromResult(GetRequestBody(request)); 
        }

        public WorkloadResponse GetRequestBody(WorkloadRequest content)
        {
            string val = "";
            int numberBatch = 0;
            WorkloadResponse response = new WorkloadResponse();

            List<CommunicationServer.Models.Workload> wkldList = new List<CommunicationServer.Models.Workload>();

            //choose which file to read from
            if (content.BenchMark == WorkloadRequest.Types.BenchMarkType.Dvdtest)
                wkldList = Data.DVDTest;
            else if (content.BenchMark == WorkloadRequest.Types.BenchMarkType.Dvdtrain)
                wkldList = Data.DVDTrain;
            else if (content.BenchMark == WorkloadRequest.Types.BenchMarkType.NdbenchTest)
                wkldList = Data.NDBTest;
            else if (content.BenchMark == WorkloadRequest.Types.BenchMarkType.NdbenchTrain)
                wkldList = Data.NDBTrain;
            else
            {
                response.Rfw = "Benchmark doesnot exist";
                return response;
            }

            int totalCount = ListCount(ref wkldList);

            if (content.BatchUnit != 0)
            {
                decimal tmp = totalCount / content.BatchUnit;
                //round total number of batch
                numberBatch = (int)Math.Ceiling(tmp);
            }

            int startID = content.BatchUnit * content.BatchId;

            if (content.BatchId > numberBatch)
                startID = 0;

            int start = startID;
            for (int j = content.BatchId; j < content.BatchId + content.BatchSize; j++)
            {
                Batch batch = new Batch();
                batch.BatchId = j;
                for (int i = start; i < start + content.BatchUnit; i++)
                {
                    batch.Values.Add(wkldList[i].MemoryUtilization_Average);
                }
                start += content.BatchUnit;
                response.Batches.Add(batch);
            }
            response.LastBatchId = (content.BatchId + content.BatchSize - 1).ToString();
            response.Rfw = content.Rfw;

            return response;
        }


        /// <summary>
        /// Get total number of line in a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int ListCount(ref List<CommunicationServer.Models.Workload> listWorkload)
        {
            return listWorkload.Count;
        }
    }
}
