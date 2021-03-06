using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer.Models
{
    public class StatAgent
    {
        public int Dbid { get; set; }
        public string EmployeeId { get; set; }
        public string Site { get; set; }
        public int NbEmail { get; set; }
        public int NbTacheUploadDoc { get; set; }
        public int NbTacheMevo { get; set; }
        public int NbTacheSmartphone { get; set; }
        public int NbTacheDeclanet { get; set; }
        public int NbTacheRappel { get; set; }
        public int NbTacheAutre { get; set; }

        public StatAgent()
        {
            NbEmail = 0;
            NbTacheUploadDoc = 0;
            NbTacheMevo = 0;
            NbTacheSmartphone = 0;
            NbTacheDeclanet = 0;
            NbTacheRappel = 0;
            NbTacheAutre = 0;
        }

        public void AddInteraction(string media, string tasktype)
        {
            if (media.Equals("email"))
                NbEmail++;
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
                NbEmail--;
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
                    default:
                        NbTacheAutre--;
                        break;
                }
            }

        }
    }
}
