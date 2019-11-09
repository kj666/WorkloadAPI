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

        public override Task<WorkloadResponse> GetWorkload(WorkloadRequest request, ServerCallContext context)
        {
            return Task.FromResult(GetRequestBody(request)); 
        }

        public WorkloadResponse GetRequestBody(WorkloadRequest content)
        {
            int numberBatch = 0;
            //Compute batch start id
            int startID = content.BatchUnit * content.BatchId;
            WorkloadResponse response = new WorkloadResponse();
            response.Rfw = content.Rfw;
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
                response.LastBatchId = "Error: Benchmark does not exist";
                return response;
            }

            //avoid division by 0
            if (content.BatchUnit != 0)
            {
                decimal tmp = ListCount(ref wkldList) / content.BatchUnit;
                //round total number of batch
                numberBatch = (int)Math.Ceiling(tmp);
            }

           
            if (content.BatchId > numberBatch)
            {
                response.LastBatchId = "Error: BatchId exceeds number of Batch";
                return response;
            }

            int start = startID;
            //Verify if batchsize ends
            int end = content.BatchId + content.BatchSize;

            int last = end;

            for (int j = content.BatchId; j < end; j++)
            {
                Batch batch = new Batch()
                {
                    BatchId = j
                };
                if (numberBatch == j)
                {
                    for (int i = start; i < wkldList.Count; i++)
                    {
                        batch.Values.Add(VerifyMetric(wkldList[i], content.Metric));
                    }
                    last = numberBatch + 1;
                    start += content.BatchUnit;
                    response.Batches.Add(batch);
                    break;
                }
                else
                {
                    for (int i = start; i < start + content.BatchUnit; i++)
                    {
                        batch.Values.Add(VerifyMetric(wkldList[i], content.Metric));
                    }
                }
                start += content.BatchUnit;
                response.Batches.Add(batch);
            }
            response.LastBatchId = (last - 1).ToString();
            
            return response;
        }

        public double VerifyMetric(CommunicationServer.Models.Workload work, WorkloadRequest.Types.MetricType type)
        {
            double result = 0;
            if (type == WorkloadRequest.Types.MetricType.Cpu)
                result = work.CPUUtilization_Average;
            else if(type == WorkloadRequest.Types.MetricType.NetworkIn)
                result = work.NetworkIn_Average;
            else if (type == WorkloadRequest.Types.MetricType.NetworkOut)
                result = work.NetworkOut_Average;
            else if (type == WorkloadRequest.Types.MetricType.Memory)
                result = work.MemoryUtilization_Average;
            else if (type == WorkloadRequest.Types.MetricType.FinalTarget)
                result = work.FinalTarget;

            return result;
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
