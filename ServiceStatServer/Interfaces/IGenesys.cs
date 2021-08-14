using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer.Interfaces
{
    public interface IGenesys
    {
        bool CnxConfServer();
        void DecnxConfServer();
        bool CnxIxnServer();
        void DecnxIxnServer();
        void GetAgents();
    }
}
