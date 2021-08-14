using Microsoft.Extensions.Hosting;
using ServiceStatServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceStatServer.Services
{
    public class Worker : BackgroundService
    {
        private readonly IGenesys _genesys;
        private readonly IDAL _dal;

        public Worker(IGenesys genesys, IDAL dal)
        {
            _genesys = genesys;
            _dal = dal;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            // Récupération des agents
            if (_genesys is not null)
            {
                if (_genesys.CnxConfServer())
                {
                    _genesys.GetAgents();
                    _genesys.DecnxConfServer();
                }
            }

            if (_dal.Connexion())
            {
                _dal.GetIxnData();
                _dal.Deconnexion();
            }

            if (_genesys is not null)
            {
                _genesys.CnxIxnServer();
            }

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_genesys is not null)
            {
                _genesys.DecnxIxnServer();
            }

            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
