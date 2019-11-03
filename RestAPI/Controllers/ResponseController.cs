using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Workload.Models;
using Workload;
using System.Text.Json.Serialization;

namespace Workload.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        [HttpGet]
        public RfwResponse GetRequestBody([FromBody] RfwRequest content)
        {
            string val = "";
            int numberBatch = 0;
            int startID = 0;
            int totalCount = 0;
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

            totalCount = ListCount(ref wkldList);
            
            if (content.BatchUnit != 0)
            {
                decimal tmp = totalCount / content.BatchUnit;
                //round total number of batch
                numberBatch = (int)Math.Ceiling(tmp);
            }

            startID = content.BatchUnit * content.BatchId;

            if (content.BatchId > numberBatch)
                startID = 0;

            val = "TotalCount: " + totalCount +
                  "\nBatchCount: " + numberBatch +
                  "\nStartID: "+startID +
                  "\nBatchId: " + content.BatchId;

            int start = startID;
            for (int j = content.BatchId; j < content.BatchId + content.BatchSize; j++)
            {
                Batch batch = new Batch();
                batch.Id = j;
                batch.Workloads = new List<Models.Workload>();
                for (int i = start; i < start + content.BatchUnit; i++)
                {
                    batch.Workloads.Add(wkldList[i]);
                }
                start += content.BatchUnit;
                response.Batches.Add(batch);
            }
            response.LastBatchId = content.BatchId + content.BatchSize - 1;
            response.RfwID = val;

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
    }
}
