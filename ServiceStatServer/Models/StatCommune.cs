using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer.Models
{
    public class StatCommune
    {
        public string Place { get; set; }
        public string Workbin { get; set; }
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

        public StatCommune()
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
        }
        public void AddInteraction(string media, string tasktype)
        {
            if (media.Equals("email"))
            {
                switch (tasktype)
                {
                    case "FAX":
                        NbFax++;
                        break;
                    default:
                        NbEmail++;
                        break;
                }
            }
            else
            {
                switch (tasktype)
                {
                    case "UPLOADDOC":
                        NbTacheUploadDoc++;
                        break;
                    case "MEVO":
                        NbTacheMevo++;
                        break;
                    case "SMARTPHONE":
                    case "DECLAPHONE":
                        NbTacheSmartphone++;
                        break;
                    case "DECLANET":
                        NbTacheDeclanet++;
                        break;
                    case "RAPPEL":
                        NbTacheRappel++;
                        break;
                    case "ECONSTAT":
                        NbTacheEConstat++;
                        break;
                    case "MAF":
                        NbTacheMAF++;
                        break;
                    default:
                        NbTacheAutre++;
                        break;
                }
            }

        }

        public void SupprimeInteraction(string media, string tasktype)
        {
            if (media.Equals("email"))
            {
                switch (tasktype)
                {
                    case "FAX":
                        NbFax--;
                        break;
                    default:
                        NbEmail--;
                        break;
                }
            }
            else
            {
                switch (tasktype)
                {
                    case "UPLOADDOC":
                        NbTacheUploadDoc--;
                        break;
                    case "MEVO":
                        NbTacheMevo--;
                        break;
                    case "SMARTPHONE":
                    case "DECLAPHONE":
                        NbTacheSmartphone--;
                        break;
                    case "DECLANET":
                        NbTacheDeclanet--;
                        break;
                    case "RAPPEL":
                        NbTacheRappel--;
                        break;
                    case "ECONSTAT":
                        NbTacheEConstat--;
                        break;
                    case "MAF":
                        NbTacheMAF--;
                        break;
                    default:
                        NbTacheAutre--;
                        break;
                }
            }
        }
    }
}
