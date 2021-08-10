using Microsoft.Extensions.Logging;
using ServiceStatServer.Interfaces;
using ServiceStatServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer.Services
{
    public class Donnees : IDonnees 
    {
        private readonly ILogger<StatService> _logger;

        private List<Agent> ListeAgents;
        private List<Interaction> ListeInteractions;
        private List<StatAgent> ListeStatAgents;

        public Donnees(ILogger<StatService> logger)
        {
            _logger = logger;

            ListeAgents = new List<Agent>();
            ListeInteractions = new List<Interaction>();
            ListeStatAgents = new List<StatAgent>();
        }

        public void AjoutAgent(int dbid, string employeeid, string firstname, string lastname, string supid)
        {
            Agent agent = new Agent { Dbid = dbid, EmployeeId = employeeid, FirstName = firstname, LastName = lastname, SupID = supid };

            ListeAgents.Add(agent);

            ListeStatAgents.Add(new StatAgent { Dbid = dbid, EmployeeId = employeeid});
        }

        public void AjoutInteraction(string ixnid, string workbin, string employeeid, string media)
        {
            // On ne l'ajoute que si elle n'existe pas (cas d'évènements multiples)
            Interaction interaction = ListeInteractions.Find(i => i.IxnId.Equals(ixnid));
            if (interaction == null)
            {
                ListeInteractions.Add(new Interaction { IxnId = ixnid, Workbin = workbin, AgentId = employeeid, Media = media });

                // On augmente la stat de l'agent concerné
                StatAgent statAgent = ListeStatAgents.Find(s => s.EmployeeId.Equals(employeeid));
                if (statAgent != null)
                {
                    if (media.Equals("email"))
                        statAgent.NbEmail++;
                    else
                        statAgent.NbTache++;
                }
            }
        }

        public string GetNbIxnWorkbinAgent(int dbid)
        {
            string resultat = "0";

            StatAgent statAgent = ListeStatAgents.Find(s => s.Dbid == dbid);
            if (statAgent != null)
                resultat = (statAgent.NbEmail + statAgent.NbTache).ToString();
            /*
            Agent agent = ListeAgents.Find(a => a.Dbid == dbid);
            if (agent != null)
            {
                List<Interaction> interactions = ListeInteractions.FindAll(i => i.AgentId.Equals(agent.EmployeeID));
                resultat = interactions.Count.ToString();
            }
            */

            return resultat;
        }
    }
}
