using Grpc.Core;
using Microsoft.Extensions.Logging;
using ServiceStatServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer
{
    public class StatService : Stat.StatBase
    {
        private readonly ILogger<StatService> _logger;
        private readonly IDonnees _donnees;

        public StatService(ILogger<StatService> logger, IDonnees donnees)
        {
            _logger = logger;
            _donnees = donnees;
        }

        public override Task<StatReply> GetStat(StatRequest request, ServerCallContext context)
        {
            return Task.FromResult(new StatReply
            {
                Value = _donnees.GetNbIxnWorkbinAgent(int.Parse(request.Object))
            }) ;
        }
    }
}
