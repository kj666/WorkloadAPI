using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Workload.Models;

namespace Workload.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        [HttpGet]
        public RfwResponse GetRequestBody([FromBody] RfwRequest content)
        {
            int numberBatch = 0;
            int startID = content.BatchUnit * content.BatchId;
            RfwResponse response = new RfwResponse();
            response.Batches = new List<Batch>();
            List<Workload.Models.Workload> wkldList = new List<Workload.Models.Workload>();

            //choose which file to read from
            if (content.BenchmarkType == BenchMarkType.DVDTest)
                wkldList = Data.DVDTest;
            else if (content.BenchmarkType == BenchMarkType.DVDTrain)
                wkldList = Data.DVDTrain;
            else if (content.BenchmarkType == BenchMarkType.NDBenchTest)
                wkldList = Data.NDBTest;
            else if (content.BenchmarkType == BenchMarkType.NDBenchTrain)
                wkldList = Data.NDBTrain;
            else
            {
                response.RfwID = "BenchmarkType " + content.BenchmarkType + " does not exist";
                return response;
            }
            
            if (content.BatchUnit != 0)
            {
                decimal tmp = ListCount(ref wkldList) / content.BatchUnit;
                //round total number of batch
                numberBatch = (int)Math.Ceiling(tmp);
            }

            if (content.BatchId > numberBatch)
            {
                response.RfwID = "Error: BatchId exceeds number of Batch";
                return response;
                //startID = 0;
            }

            if(content.WorloadMetric.GetHashCode() > 4)
            {
                response.RfwID = "Error: Metric Type does not exists";
                return response;
            }

            int start = startID;
            int end = content.BatchId + content.BatchSize;

            int last = end;

            for (int j = content.BatchId; j < end; j++)
            {
                Batch batch = new Batch();
                batch.Id = j;
                batch.values = new List<double>();
                if (j == numberBatch)
                {
                    for (int i = start; i < wkldList.Count; i++)
                    {
                        batch.values.Add(VerifyMetric(wkldList[i], content.WorloadMetric));
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
                        batch.values.Add(VerifyMetric(wkldList[i], content.WorloadMetric));
                    }
                }
                start += content.BatchUnit;
                response.Batches.Add(batch);
            }

            response.LastBatchId = last-1;
            response.RfwID = content.RfwID;

            return response;
        }


        [HttpGet("all")]
        public List<Models.Workload> GetResponse()
        {
            return Data.DVDTrain;
        }

        /// <summary>
        /// Get total number of line in a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int ListCount(ref List<Workload.Models.Workload> listWorkload)
        {
            return listWorkload.Count;
        }

        public double VerifyMetric(Workload.Models.Workload work, Workload.Models.WorkloadType metric)
        {
            double result = 0;

            if (metric == Workload.Models.WorkloadType.CPU)
                result = work.CPUUtilization_Average;
            else if (metric == Workload.Models.WorkloadType.NetworkIn)
                result = work.NetworkIn_Average;
            else if (metric == Workload.Models.WorkloadType.NetworkOut)
                result = work.NetworkOut_Average;
            else if (metric == Workload.Models.WorkloadType.Memory)
                result = work.MemoryUtilization_Average;
            else if (metric == Workload.Models.WorkloadType.FinalTarget)
                result = work.FinalTarget;

            return result;
        }

    }
}
