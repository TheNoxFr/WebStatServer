using Grpc.Net.Client;
using ServiceStatServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSStatServer.Interfaces;

namespace WSStatServer.Services
{
    public class Connexion : IConnexion
    {
        public Stat.StatClient client { get; }

        public Connexion(IGRPCSettings settings)
        {
            var channel = GrpcChannel.ForAddress("https://" + settings.Host + ":" + settings.Port);
            this.client = new Stat.StatClient(channel);
        }

    }
}
