using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Workload.Models;

namespace Workload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        string path_DVD_Test = "C:/dev/Workload/Data/DVD-testing.csv";
        string path_DVD_Train = "C:/dev/Workload/Data/DVD-training.csv";
        string path_NDB_Test = "C:/dev/Workload/Data/NDBench-testing.csv";
        string path_NDB_Train = "C:/dev/Workload/Data/NDBench-training.csv";

        // GET: api/Response
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Response/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return ParseCsv();
        }

        public string ParseCsv() { 

            string[] lines = System.IO.File.ReadAllLines(path_DVD_Test);
            string output = "";
            int count = 0;
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                foreach(string column in columns)
                {
                    output = (output +" "+column);
                }
                output = output + "\n";
                count++;
                if (count == 10)
                    break;
            }
            return output;
        }

        [HttpGet("Request")]
        public int GetRequestBody([FromBody] RfwRequest content)
        {
           
            string path = "";
            //choose which file to read from
            if (content.BenchmarkType == BenchMarkType.DVDTest)
                path = path_DVD_Test;
            else if (content.BenchmarkType == BenchMarkType.DVDTrain)
                path = path_DVD_Train;
            else if (content.BenchmarkType == BenchMarkType.NDBenchTest)
                path = path_NDB_Test;
            else if (content.BenchmarkType == BenchMarkType.NDBenchTrain)
                path = path_NDB_Train;

            int totalLine = GetNumberLine(path);
            int numberBatch = 0;
            int lineStart = 0;

            if (content.BatchUnit != 0)
            {
                decimal tmp = totalLine / content.BatchUnit;
                numberBatch = (int)Math.Ceiling(tmp);
            }

            lineStart = content.BatchUnit * content.BatchId;
            if (content.BatchId > numberBatch)
                lineStart = 0;

            return lineStart;
        }


        [HttpGet("Response")]
        public List<Models.Workload> GetResponse()
        {
            return Data.DVDTrain;
        }
        /// <summary>
        /// Get total number of line in a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int GetNumberLine(string path)
        {
            return System.IO.File.ReadAllLines(path).Count();
        }
        // POST: api/Response
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Response/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
