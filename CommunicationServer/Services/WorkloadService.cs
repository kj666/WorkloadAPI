using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public override Task<WorkloadReply> GetWorkload(WorkloadRequest request, ServerCallContext context)
        {

            return Task.FromResult(new WorkloadReply
            {
                Message = "Hello " + request
            });
        }

    }
}
