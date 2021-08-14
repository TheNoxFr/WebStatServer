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
         //   _logger.LogInformation("GetStat");
            switch (request.Stat)
            {
                case "NbEmailWorkbin" :
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbEmailWorkbinAgent(int.Parse(request.Object)) });
                case "NbTaskWorkbin" :
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbTaskWorkbinAgent(int.Parse(request.Object)) });
                case "NbTaskUploadDocWorkbin":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbTaskUploadDocWorkbinAgent(int.Parse(request.Object)) });
                case "NbTaskMevoWorkbin":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbTaskMevoWorkbinAgent(int.Parse(request.Object)) });
                case "NbTaskSmartphoneWorkbin":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbTaskSmartphoneWorkbinAgent(int.Parse(request.Object)) });
                case "NbTaskDeclanetWorkbin":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbTaskDeclanetWorkbinAgent(int.Parse(request.Object)) });
                case "NbTaskRappelWorkbin":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbTaskRappelWorkbinAgent(int.Parse(request.Object)) });
                case "NbTaskAutreWorkbin":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbTaskAutreWorkbinAgent(int.Parse(request.Object)) });

                case "NbIxnAIdentifier":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbAIdentifierWorkbinCommune(request.Object) });
                case "NbIxnDLT":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbDLTWorkbinCommune(request.Object) }); 
                case "NbIxnAAffecter":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbAAffecterWorkbinCommune(request.Object) });
                case "NbIxnAbsent":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbAbsentWorkbinCommune(request.Object) });

                case "NbIxnWorkbinSite":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbWorkbinSite(request.Object) });
                case "NbIxnEchuesWorkbinSite":
                    return Task.FromResult(new StatReply { Value = _donnees.GetNbEchuesWorkbinSite(request.Object) });

                case "SupId":
                    return Task.FromResult(new StatReply { Value = _donnees.GetIdSup(int.Parse(request.Object)) });
            }

            return Task.FromResult(new StatReply
            {
                Value = "0"
            }) ;
        }
    }
}
