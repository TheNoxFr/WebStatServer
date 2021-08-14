using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer.Models
{
    public class StatSite
    {
        public string Site { get; set; }
        public int NbEmail { get; set; }
        public int NbFax { get; set; }
        public int NbTacheUploadDoc { get; set; }
        public int NbTacheMevo { get; set; }
        public int NbTacheSmartphone { get; set; }
        public int NbTacheDeclanet { get; set; }
        public int NbTacheRappel { get; set; }
        public int NbTacheEConstat { get; set; }
        public int NbTacheMAF { get; set; }
        public int NbTacheAutre { get; set; }
        public int NbEmailEcheance { get; set; }
        public int NbFaxEcheance { get; set; }
        public int NbTacheUploadDocEcheance { get; set; }
        public int NbTacheMevoEcheance { get; set; }
        public int NbTacheSmartphoneEcheance { get; set; }
        public int NbTacheDeclanetEcheance { get; set; }
        public int NbTacheRappelEcheance { get; set; }
        public int NbTacheEConstatEcheance { get; set; }
        public int NbTacheMAFEcheance { get; set; }
        public int NbTacheAutreEcheance { get; set; }

        public StatSite()
        {
            NbEmail = 0;
            NbFax = 0;
            NbTacheUploadDoc = 0;
            NbTacheMevo = 0;
            NbTacheSmartphone = 0;
            NbTacheDeclanet = 0;
            NbTacheRappel = 0;
            NbTacheEConstat = 0;
            NbTacheMAF = 0;
            NbTacheAutre = 0;
            NbEmailEcheance = 0;
            NbFaxEcheance = 0;
            NbTacheUploadDocEcheance = 0;
            NbTacheMevoEcheance = 0;
            NbTacheSmartphoneEcheance = 0;
            NbTacheDeclanetEcheance = 0;
            NbTacheRappelEcheance = 0;
            NbTacheEConstatEcheance = 0;
            NbTacheMAFEcheance = 0;
            NbTacheAutreEcheance = 0;
        }

        public void AddInteraction(string media, string tasktype, string echeance)
        {
            int isEcheance = echeance.Equals("1") ? 1 : 0;

            if (media.Equals("email"))
            {
                switch (tasktype)
                {
                    case "FAX":
                        NbFax++;
                        NbFaxEcheance += isEcheance;
                        break;
                    default:
                        NbEmail++;
                        NbEmailEcheance += isEcheance;
                        break;
                }
            }
            else
            {
                switch (tasktype)
                {
                    case "UPLOADDOC":
                        NbTacheUploadDoc++;
                        NbTacheUploadDocEcheance += isEcheance;
                        break;
                    case "MEVO":
                        NbTacheMevo++;
                        NbTacheMevoEcheance += isEcheance;
                        break;
                    case "SMARTPHONE":
                    case "DECLAPHONE":
                        NbTacheSmartphone++;
                        NbTacheSmartphoneEcheance += isEcheance;
                        break;
                    case "DECLANET":
                        NbTacheDeclanet++;
                        NbTacheDeclanetEcheance += isEcheance;
                        break;
                    case "RAPPEL":
                        NbTacheRappel++;
                        NbTacheRappelEcheance += isEcheance;
                        break;
                    case "ECONSTAT":
                        NbTacheEConstat++;
                        NbTacheEConstatEcheance += isEcheance;
                        break;
                    case "MAF":
                        NbTacheMAF++;
                        NbTacheMAFEcheance += isEcheance;
                        break;
                    default:
                        NbTacheAutre++;
                        NbTacheAutreEcheance += isEcheance;
                        break;
                }
            }

        }

        public void SupprimeInteraction(string media, string tasktype, string echeance)
        {
            int isEcheance = echeance.Equals("1") ? 1 : 0;

            if (media.Equals("email"))
            {
                switch (tasktype)
                {
                    case "FAX":
                        NbFax--;
                        NbFaxEcheance -= isEcheance;
                        break;
                    default:
                        NbEmail--;
                        NbEmailEcheance -= isEcheance;
                        break;
                }
            }
            else
            {
                switch (tasktype)
                {
                    case "UPLOADDOC":
                        NbTacheUploadDoc--;
                        NbTacheUploadDocEcheance -= isEcheance;
                        break;
                    case "MEVO":
                        NbTacheMevo--;
                        NbTacheMevoEcheance -= isEcheance;
                        break;
                    case "SMARTPHONE":
                    case "DECLAPHONE":
                        NbTacheSmartphone--;
                        NbTacheSmartphoneEcheance -= isEcheance;
                        break;
                    case "DECLANET":
                        NbTacheDeclanet--;
                        NbTacheDeclanetEcheance -= isEcheance;
                        break;
                    case "RAPPEL":
                        NbTacheRappel--;
                        NbTacheRappelEcheance -= isEcheance;
                        break;
                    case "ECONSTAT":
                        NbTacheEConstat--;
                        NbTacheEConstatEcheance -= isEcheance;
                        break;
                    case "MAF":
                        NbTacheMAF--;
                        NbTacheMAFEcheance -= isEcheance;
                        break;
                    default:
                        NbTacheAutre--;
                        NbTacheAutreEcheance -= isEcheance;
                        break;
                }
            }
        }
    }
}
