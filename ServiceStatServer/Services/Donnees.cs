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
        private List<Agent> ListeAgents;
        private List<Interaction> ListeInteractions;

        public Donnees()
        {
            ListeAgents = new List<Agent>();
            ListeInteractions = new List<Interaction>();
        }

        public void AjoutAgent(int dbid, string employeeid, string firstname, string lastname, string supid)
        {
            Agent agent = new Agent { Dbid = dbid, EmployeeID = employeeid, FirstName = firstname, LastName = lastname, SupID = supid };

            ListeAgents.Add(agent);
        }

        public void AjoutInteraction(string ixnid, string workbin, string employeeid)
        {
            ListeInteractions.Add(new Interaction { IxnId = ixnid, Workbin = workbin, AgentId = employeeid });
        }

        public string GetNbIxnWorkbinAgent(int dbid)
        {
            string resultat = "0";

            Agent agent = ListeAgents.Find(a => a.Dbid == dbid);
            if (agent != null)
            {
                List<Interaction> interactions = ListeInteractions.FindAll(i => i.AgentId.Equals(agent.EmployeeID));
                resultat = interactions.Count.ToString();
            }

            return resultat;
        }
    }
}
