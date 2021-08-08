using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer.Interfaces
{
    public interface IDonnees
    {
        void AjoutAgent(int dbid, string employeeid, string firstname, string lastname, string supid);

        void AjoutInteraction(string ixnid, string workbin, string employeeid);

        string GetNbIxnWorkbinAgent(int dbid);
    }
}
