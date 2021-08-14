using Genesyslab.Platform.ApplicationBlocks.ConfigurationObjectModel;
using Genesyslab.Platform.ApplicationBlocks.ConfigurationObjectModel.CfgObjects;
using Genesyslab.Platform.ApplicationBlocks.ConfigurationObjectModel.Queries;
using Genesyslab.Platform.Commons.Collections;
using Genesyslab.Platform.Commons.Protocols;
using Genesyslab.Platform.Configuration.Protocols;
using Genesyslab.Platform.Configuration.Protocols.Types;
using Genesyslab.Platform.OpenMedia.Protocols;
using Genesyslab.Platform.OpenMedia.Protocols.InteractionServer;
using Genesyslab.Platform.OpenMedia.Protocols.InteractionServer.Events;
using Genesyslab.Platform.OpenMedia.Protocols.InteractionServer.Requests.AgentManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStatServer.Interfaces;
using ServiceStatServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer.Services
{
    public class Genesys : IGenesys
    {
        private readonly IDonnees _donnees;
        private readonly ILogger<StatService> _logger;
        private readonly IConfiguration _configuration;

        private ConfServerProtocol confServerProtocol;
        private IConfService confService;
        private string confServerHost;
        private int confServerPort;
        private string confServerName;
        private string confServerUser;
        private string confServerPassword;

        private InteractionServerProtocol ISChannel;
        private string ixnServerHost;
        private int ixnServerPort;
        private string ixnServerClientName;
        private string tenantId;

        public Genesys(IDonnees donnees, ILogger<StatService> logger, IConfiguration configuration)
        {
            _donnees = donnees;
            _logger = logger;
            _configuration = configuration;

            confServerName = _configuration["MyConfig:ConfigServer:AppName"];
            confServerHost = _configuration["MyConfig:ConfigServer:Host"];
            confServerPort = int.Parse(_configuration["MyConfig:ConfigServer:Port"]);
            confServerUser = _configuration["MyConfig:ConfigServer:User"];
            confServerPassword = _configuration["MyConfig:ConfigServer:Password"];

            ixnServerHost = _configuration["MyConfig:IxnServer:Host"];
            ixnServerPort = int.Parse(_configuration["MyConfig:IxnServer:Port"]);
            ixnServerClientName = _configuration["MyConfig:IxnServer:WSCCPulse"];
            tenantId = _configuration["MyConfig:IxnServer:TenantId"];

        }

        #region ConfServer
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

            foreach (CfgPerson p in listeAgents)
            {
                string supid = "";
                string site = "";
                foreach(CfgSkillLevel sl in p.AgentInfo.SkillLevels)
                {
                    if (sl.Skill.Name.Contains("F_SUP"))
                    {
                        supid = sl.Skill.Name.Substring(6);
                    } else if (sl.Skill.Name.Contains("T_Site_"))
                    {
                        site = sl.Skill.Name.Substring(7);
                    }
                }

                _donnees.AjoutAgent(p.DBID, p.EmployeeID, p.FirstName, p.LastName, supid, site);
            }
        }
        #endregion

        #region IxnServer
        public bool CnxIxnServer()
        {
            ISChannel = new InteractionServerProtocol(new Endpoint(ixnServerHost, ixnServerPort));
            ISChannel.ClientType = InteractionClient.ReportingEngine;
            ISChannel.ClientName = ixnServerClientName;

            ISChannel.Opened += new EventHandler(OnChannelOpened);
            ISChannel.Received += ISChannel_Received;

            try
            {
                ISChannel.Open();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void OnChannelOpened(object sender, EventArgs eventArgs)
        {
            KeyValueCollection tenantList = new KeyValueCollection();
            KeyValueCollection allWorkbin = new KeyValueCollection();
            allWorkbin.Add("Workbins", "");
            tenantList.Add(tenantId,allWorkbin);

            RequestStartPlaceAgentStateReportingAll request = RequestStartPlaceAgentStateReportingAll.Create(tenantList);

            ISChannel.Request(request);
        }

        private void ISChannel_Received(object sender, EventArgs e)
        {
            IMessage message = ((MessageEventArgs)e).Message;
            _logger.LogInformation("Genesys:ISChannel_Received " + message.Name);

            switch (message.Id)
            {
                case EventPlacedInWorkbin.MessageId:
                    PlacedInWorkbin((EventPlacedInWorkbin)message);
                    break;
                case EventTakenFromWorkbin.MessageId:
                    TakenFromWorkbin((EventTakenFromWorkbin)message);
                    break;
                case EventProcessingStopped.MessageId:
                    ProcessingStopped((EventProcessingStopped)message);
                    break;
            }
        }

        private void ProcessingStopped(EventProcessingStopped message)
        {
            string id = message.Interaction.InteractionId;

            _logger.LogInformation("Genesys:ProcessingStopped " + id);
            _donnees.SupprimeInteraction(id);
        }

        private void TakenFromWorkbin(EventTakenFromWorkbin message)
        {
            string id = message.Interaction.InteractionId;

            _logger.LogInformation("Genesys:TakenFromWorkbin " + id);
            _donnees.SupprimeInteraction(id);
        }
        private void PlacedInWorkbin(EventPlacedInWorkbin message)
        {
            string id = message.Interaction.InteractionId;
            string media = message.Interaction.InteractionMediatype;
            string agentid = message.Interaction.InteractionAgentId;
            string placeid = message.Interaction.InteractionPlaceId;
            string workbin = message.Interaction.InteractionWorkbinTypeId;
            string tasktype = message.Interaction.InteractionUserData.GetAsString("R_AT");
            string echeance = message.Interaction.InteractionUserData.GetAsString("A_ALERTE_ECHEANCE");

            _logger.LogInformation("Genesys:PlacedInWorkbin " + id);
            _donnees.AjoutInteraction(id, workbin, agentid, placeid, media, tasktype, echeance);
        }
        public void DecnxIxnServer()
        {
            try
            {
                ISChannel.Close();
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
