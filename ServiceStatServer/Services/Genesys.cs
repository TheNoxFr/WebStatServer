using Genesyslab.Platform.ApplicationBlocks.ConfigurationObjectModel;
using Genesyslab.Platform.ApplicationBlocks.ConfigurationObjectModel.CfgObjects;
using Genesyslab.Platform.ApplicationBlocks.ConfigurationObjectModel.Queries;
using Genesyslab.Platform.Commons.Protocols;
using Genesyslab.Platform.Configuration.Protocols;
using Genesyslab.Platform.Configuration.Protocols.Types;
using ServiceStatServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer.Services
{
    public class Genesys : IGenesys
    {
        private readonly IDonnees _donnees;

        private ConfServerProtocol confServerProtocol;
        private IConfService confService;
        private string confServerHost;
        private int confServerPort;
        private string confServerName;
        private string confServerUser;
        private string confServerPassword;

        public Genesys(IDonnees donnees)
        {
            _donnees = donnees;

            confServerName = "default";
            confServerHost = "genserv";
            confServerPort = 2020;
            confServerUser = "default";
            confServerPassword = "password";
        }

        public bool CnxConfServer()
        {
            Endpoint cfgServerEndPoint = new Endpoint(confServerName, confServerHost, confServerPort);
            confServerProtocol = new ConfServerProtocol(cfgServerEndPoint);
            confServerProtocol.ClientApplicationType = (int)CfgAppType.CFGSCE;
            confServerProtocol.ClientName = confServerName;
            confServerProtocol.UserName = confServerUser;
            confServerProtocol.UserPassword = confServerPassword;

            try
            {
                confServerProtocol.Open();
                confService = ConfServiceFactory.CreateConfService(confServerProtocol);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void DecnxConfServer()
        {
            if (confServerProtocol != null)
            {
                confServerProtocol.Close();
            }
        }

        public void GetAgents()
        {
            CfgPersonQuery query = new CfgPersonQuery();
            query.IsAgent = 0;

            ICollection<CfgPerson> listeAgents = confService.RetrieveMultipleObjects<CfgPerson>(query);

            foreach(CfgPerson p in listeAgents)
            {
                _donnees.AjoutAgent(p.DBID, p.EmployeeID, p.FirstName, p.LastName, "");
            }
        }
    }
}
