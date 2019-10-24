using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Workload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        string path_DVD_Test = "C:/dev/Workload/Data/DVD-testing.csv";

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
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                foreach(string column in columns)
                {
                    output = (output +" "+column);
                }
                output = output + "\n";
            }
            return output;
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
