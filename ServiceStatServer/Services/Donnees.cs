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

        private readonly Object ixnLock = new Object();

        private List<Agent> ListeAgents;
        private List<Interaction> ListeInteractions;
        private List<StatAgent> ListeStatAgents;
        private List<StatCommune> ListeStatCommunes;
        private List<StatSite> ListeStatSites;

        public Donnees(ILogger<StatService> logger)
        {
            _logger = logger;

            ListeAgents = new List<Agent>();
            ListeInteractions = new List<Interaction>();
            ListeStatAgents = new List<StatAgent>();
            ListeStatCommunes = new List<StatCommune>();
            ListeStatSites = new List<StatSite>();
        }

        public void AjoutAgent(int dbid, string employeeid, string firstname, string lastname, string supid, string site)
        {
            _logger.LogInformation("Donnees:AjoutAgent dbid=" + dbid.ToString() + " employeeid=" + employeeid + " firstname=" + firstname + " lastname=" + lastname + " supid=" + supid);
            Agent agent = new Agent { Dbid = dbid, EmployeeId = employeeid, FirstName = firstname, LastName = lastname, SupID = supid };

            ListeAgents.Add(agent);

            ListeStatAgents.Add(new StatAgent { Dbid = dbid, EmployeeId = employeeid, Site = site});

            StatSite statSite = ListeStatSites.Find(s => s.Site.Equals(site));
            if (statSite == null)
            {
                ListeStatSites.Add(new StatSite { Site = site });
                _logger.LogInformation("Donnees:AjoutAgent Ajout de StatSite : " + site);
            }
        }

        public void SupprimeInteraction(string ixnid)
        {
            _logger.LogInformation("Donnees:SupprimeInteraction " + ixnid);

            lock(ixnLock)
            {
                Interaction interaction = ListeInteractions.Find(i => i.IxnId.Equals(ixnid));
                if (interaction != null)
                {
                    // on la supprime de la liste des interactions
                    ListeInteractions.Remove(interaction);
                    _logger.LogInformation("Donnees:SupprimeInteraction Suppression dans la liste de " + ixnid);

                    // On la décompte de la bannette de l'agent si elle était dans une bannette individuelle
                    StatAgent statAgent = ListeStatAgents.Find(s => s.EmployeeId.Equals(interaction.AgentId));
                    if (statAgent != null)
                    {
                        statAgent.SupprimeInteraction(interaction.Media, interaction.TaskType);

                        StatSite statSite = ListeStatSites.Find(s => s.Site.Equals(statAgent.Site));
                        if (statSite != null)
                        {
                            statSite.SupprimeInteraction(interaction.Media, interaction.TaskType, interaction.Echeance);
                        }

                    }
                    else // Elle était dans une bannette commune
                    {
                        StatCommune statCommune = ListeStatCommunes.Find(s => s.Workbin.Equals(interaction.Workbin) && s.Place.Equals(interaction.PlaceId));
                        if (statCommune != null)
                        {
                            statCommune.SupprimeInteraction(interaction.Media, interaction.TaskType);
                        }

                    }
                }

            }
        }
        public void AjoutInteraction(string ixnid, string workbin, string employeeid, string place, string media, string tasktype, string echeance)
        {
            _logger.LogInformation("Donnees:AjoutInteraction ixnid=" + ixnid + " workbin=" + workbin + " employyeid=" + employeeid + " place=" + place + " media=" + media + " tasktype=" + tasktype + " echeance="+ echeance);

            lock(ixnLock)
            {
                // On ne l'ajoute que si elle n'existe pas (cas d'évènements multiples)
                Interaction interaction = ListeInteractions.Find(i => i.IxnId.Equals(ixnid));
                if (interaction == null)
                {
                    ListeInteractions.Add(new Interaction { IxnId = ixnid, Workbin = workbin, AgentId = employeeid, PlaceId = place, Media = media, TaskType = tasktype, Echeance = echeance });
                    _logger.LogInformation("Donnees:AjoutInteraction Ajout dans la liste de " + ixnid);

                    // si c'est une workbin agent
                    if (employeeid != null && !employeeid.Equals(""))
                    {
                        // On augmente la stat de l'agent concerné
                        _logger.LogInformation("Donnees:AjoutInteraction WorkbinAgent " + employeeid);
                        StatAgent statAgent = ListeStatAgents.Find(s => s.EmployeeId.Equals(employeeid));
                        if (statAgent != null)
                        {
                            statAgent.AddInteraction(media, tasktype);

                            StatSite statSite = ListeStatSites.Find(s => s.Site.Equals(statAgent.Site));
                            if (statSite != null)
                            {
                                statSite.AddInteraction(media, tasktype, echeance);
                                _logger.LogInformation("Donnees:AjoutInteraction statSite " + statAgent.Site + " ajout interaction ");
                            }
                        }

                    }
                    else // si c'est une workbin commune
                    {
                        _logger.LogInformation("Donnees:AjoutInteraction WorkbinCommune " + place + " " + workbin);
                        StatCommune statCommune = ListeStatCommunes.Find(s => s.Place.Equals(place) && s.Workbin.Equals(workbin));
                        // On la crée si elle n'existe pas
                        if (statCommune == null)
                        {
                            statCommune = new StatCommune { Place = place, Workbin = workbin };
                            ListeStatCommunes.Add(statCommune);
                        }

                        statCommune.AddInteraction(media, tasktype);
                    }
                }

            }
        }

        public string GetNbIxnWorkbinAgent(int dbid)
        {
            string resultat = "0";

            StatAgent statAgent = ListeStatAgents.Find(s => s.Dbid == dbid);
            if (statAgent != null)
                resultat = (statAgent.NbEmail + statAgent.NbTacheAutre).ToString();
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

        public string GetNbEmailWorkbinAgent(int dbid)
        {
            string resultat = "0";
            StatAgent statAgent = ListeStatAgents.Find(s => s.Dbid == dbid);
            if (statAgent != null)
                resultat = statAgent.NbEmail.ToString();

            return resultat;
        }

        public string GetNbTaskWorkbinAgent(int dbid)
        {
            string resultat = "0";
            StatAgent statAgent = ListeStatAgents.Find(s => s.Dbid == dbid);
            if (statAgent != null)
                resultat = (statAgent.NbTacheUploadDoc + statAgent.NbTacheMevo + statAgent.NbTacheSmartphone + statAgent.NbTacheDeclanet + statAgent.NbTacheRappel + statAgent.NbTacheAutre).ToString();

            return resultat;
        }

        public string GetNbTaskUploadDocWorkbinAgent(int dbid)
        {
            string resultat = "0";
            StatAgent statAgent = ListeStatAgents.Find(s => s.Dbid == dbid);
            if (statAgent != null)
                resultat = statAgent.NbTacheUploadDoc.ToString();

            return resultat;
        }
        public string GetNbTaskSmartphoneWorkbinAgent(int dbid)
        {
            string resultat = "0";
            StatAgent statAgent = ListeStatAgents.Find(s => s.Dbid == dbid);
            if (statAgent != null)
                resultat = statAgent.NbTacheSmartphone.ToString();

            return resultat;
        }
        public string GetNbTaskDeclanetWorkbinAgent(int dbid)
        {
            string resultat = "0";
            StatAgent statAgent = ListeStatAgents.Find(s => s.Dbid == dbid);
            if (statAgent != null)
                resultat = statAgent.NbTacheDeclanet.ToString();

            return resultat;
        }
        public string GetNbTaskMevoWorkbinAgent(int dbid)
        {
            string resultat = "0";
            StatAgent statAgent = ListeStatAgents.Find(s => s.Dbid == dbid);
            if (statAgent != null)
                resultat = statAgent.NbTacheMevo.ToString();

            return resultat;
        }
        public string GetNbTaskRappelWorkbinAgent(int dbid)
        {
            string resultat = "0";
            StatAgent statAgent = ListeStatAgents.Find(s => s.Dbid == dbid);
            if (statAgent != null)
                resultat = statAgent.NbTacheRappel.ToString();

            return resultat;
        }
        public string GetNbTaskAutreWorkbinAgent(int dbid)
        {
            string resultat = "0";
            StatAgent statAgent = ListeStatAgents.Find(s => s.Dbid == dbid);
            if (statAgent != null)
                resultat = statAgent.NbTacheAutre.ToString();

            return resultat;
        }

        private string GetNbIxnWorkbinCommune(string ixnqueue, string workbin)
        {
            //_logger.LogInformation("Donnees:GetNbIxnWorkbinCommune " + ixnqueue + " " + workbin);
            string resultat = "0";
            if (ixnqueue.Length < 4)
                return resultat;

            string place = ixnqueue.Substring(0, 3);
            string typetask = ixnqueue.Substring(4);

            StatCommune statCommune = ListeStatCommunes.Find(s => s.Place.Equals(place) && s.Workbin.Equals(workbin));
            if (statCommune == null)
                return resultat;

            //_logger.LogInformation("Donnees:GetNbIxnWorkbinCommune " + statCommune.Place + " " + statCommune.Workbin + " " + typetask);
            switch (typetask)
            {
                case "Email":
                    resultat = statCommune.NbEmail.ToString();
                    break;
                case "Fax":
                    resultat = statCommune.NbFax.ToString();
                    break;
                case "Doc. SmartPhone":
                    resultat = statCommune.NbTacheUploadDoc.ToString();
                    break;
                case "MEVO":
                    resultat = statCommune.NbTacheMevo.ToString();
                    break;
                case "Décla. SmartPhone":
                    resultat = statCommune.NbTacheSmartphone.ToString();
                    break;
                case "Décla. E-Sinistres":
                    resultat = statCommune.NbTacheDeclanet.ToString();
                    break;
                case "Rappel Client":
                    resultat = statCommune.NbTacheRappel.ToString();
                    break;
                case "E-Constat":
                    resultat = statCommune.NbTacheEConstat.ToString();
                    break;
                case "Décla. Mondial":
                    resultat = statCommune.NbTacheMAF.ToString();
                    break;
                case "Total":
                    resultat = (statCommune.NbEmail + statCommune.NbFax + statCommune.NbTacheMAF + statCommune.NbTacheUploadDoc + statCommune.NbTacheMevo + statCommune.NbTacheSmartphone + statCommune.NbTacheDeclanet + statCommune.NbTacheRappel + statCommune.NbTacheEConstat).ToString();
                    break;
                default:
                    break;
            }

            return resultat;
        }

        public string GetNbAIdentifierWorkbinCommune(string ixnqueue)
        {
            return GetNbIxnWorkbinCommune(ixnqueue, "MM.Workbin.SINISTRE_Bannette_A_Identifier");
        }

        public string GetNbAbsentWorkbinCommune(string ixnqueue)
        {
            return GetNbIxnWorkbinCommune(ixnqueue, "MM.Workbin.SINISTRE_Bannette_Des_Absents");
        }

        public string GetNbDLTWorkbinCommune(string ixnqueue)
        {
            return GetNbIxnWorkbinCommune(ixnqueue, "MM.Workbin.SINISTRE_Bannette_En_DLT");
        }

        public string GetNbAAffecterWorkbinCommune(string ixnqueue)
        {
            return GetNbIxnWorkbinCommune(ixnqueue, "MM.Workbin.SINISTRE_Bannette_A_Affecter");
        }

        public string GetNbWorkbinSite(string ixnqueue)
        {
            string resultat = "0";
            if (ixnqueue.Length < 4)
                return resultat;

            string site = ixnqueue.Substring(0, 3);
            string typetask = ixnqueue.Substring(4);

            StatSite statSite = ListeStatSites.Find(s => s.Site.Equals(site));
            if (statSite == null)
                return resultat;

            switch (typetask)
            {
                case "Email":
                    resultat = statSite.NbEmail.ToString();
                    break;
                case "Fax":
                    resultat = statSite.NbFax.ToString();
                    break;
                case "Doc. SmartPhone":
                    resultat = statSite.NbTacheUploadDoc.ToString();
                    break;
                case "MEVO":
                    resultat = statSite.NbTacheMevo.ToString();
                    break;
                case "Décla. SmartPhone":
                    resultat = statSite.NbTacheSmartphone.ToString();
                    break;
                case "Décla. E-Sinistres":
                    resultat = statSite.NbTacheDeclanet.ToString();
                    break;
                case "Rappel Client":
                    resultat = statSite.NbTacheRappel.ToString();
                    break;
                case "E-Constat":
                    resultat = statSite.NbTacheEConstat.ToString();
                    break;
                case "Décla. Mondial":
                    resultat = statSite.NbTacheMAF.ToString();
                    break;
                case "Total":
                    resultat = (statSite.NbEmail + statSite.NbFax + statSite.NbTacheMAF + statSite.NbTacheUploadDoc + statSite.NbTacheMevo + statSite.NbTacheSmartphone + statSite.NbTacheDeclanet + statSite.NbTacheRappel + statSite.NbTacheEConstat).ToString();
                    break;
                default:
                    break;
            }

            return resultat;
        }

        public string GetNbEchuesWorkbinSite(string ixnqueue)
        {
            //_logger.LogInformation("Donnees:GetNbEchuesWorkbinSite ixnqueue=" + ixnqueue);

            string resultat = "0";

            if (ixnqueue.Length < 4)
                return resultat;

            string site = ixnqueue.Substring(0, 3);
            string typetask = ixnqueue.Substring(4);

            StatSite statSite = ListeStatSites.Find(s => s.Site.Equals(site));
            if (statSite == null)
                return resultat;

            switch (typetask)
            {
                case "Email":
                    resultat = statSite.NbEmailEcheance.ToString();
                    break;
                case "Fax":
                    resultat = statSite.NbFaxEcheance.ToString();
                    break;
                case "Doc. SmartPhone":
                    resultat = statSite.NbTacheUploadDocEcheance.ToString();
                    break;
                case "MEVO":
                    resultat = statSite.NbTacheMevoEcheance.ToString();
                    break;
                case "Décla. SmartPhone":
                    resultat = statSite.NbTacheSmartphoneEcheance.ToString();
                    break;
                case "Décla. E-Sinistres":
                    resultat = statSite.NbTacheDeclanetEcheance.ToString();
                    break;
                case "Rappel Client":
                    resultat = statSite.NbTacheRappelEcheance.ToString();
                    break;
                case "E-Constat":
                    resultat = statSite.NbTacheEConstatEcheance.ToString();
                    break;
                case "Décla. Mondial":
                    resultat = statSite.NbTacheMAFEcheance.ToString();
                    break;
                case "Total":
                    resultat = (statSite.NbEmailEcheance + statSite.NbFaxEcheance + statSite.NbTacheMAFEcheance + statSite.NbTacheUploadDocEcheance + statSite.NbTacheMevoEcheance + statSite.NbTacheSmartphoneEcheance + statSite.NbTacheDeclanetEcheance + statSite.NbTacheRappelEcheance + statSite.NbTacheEConstatEcheance).ToString();
                    break;
                default:
                    break;
            }

            return resultat;
        }


        public string GetIdSup(int dbid)
        {
            string resultat = "0";
            Agent agent = ListeAgents.Find(a => a.Dbid == dbid);
            if (agent != null)
                resultat = agent.SupID;

            return resultat;

        }

    }
}
