using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer.Interfaces
{
    public interface IDonnees
    {
        void AjoutAgent(int dbid, string employeeid, string firstname, string lastname, string supid, string site);

        void AjoutInteraction(string ixnid, string workbin, string employeeid, string place, string media, string tasktype, string echeance);

        void SupprimeInteraction(string ixnid);
        string GetNbIxnWorkbinAgent(int dbid);

        string GetNbEmailWorkbinAgent(int dbid);
        string GetNbTaskWorkbinAgent(int dbid);
        string GetNbTaskUploadDocWorkbinAgent(int dbid);
        string GetNbTaskSmartphoneWorkbinAgent(int dbid);
        string GetNbTaskDeclanetWorkbinAgent(int dbid);
        string GetNbTaskMevoWorkbinAgent(int dbid);
        string GetNbTaskRappelWorkbinAgent(int dbid);
        string GetNbTaskAutreWorkbinAgent(int dbid);

        string GetNbAIdentifierWorkbinCommune(string ixnqueue);
        string GetNbAbsentWorkbinCommune(string ixnqueue);
        string GetNbDLTWorkbinCommune(string ixnqueue);
        string GetNbAAffecterWorkbinCommune(string ixnqueue);

        string GetNbWorkbinSite(string ixnqueue);
        string GetNbEchuesWorkbinSite(string ixnqueue);

        string GetIdSup(int dbid);
    }
}
