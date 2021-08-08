using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStatServer;

namespace WSStatServer.Interfaces
{
    public interface IConnexion
    {
        Stat.StatClient client { get; }
    }
}
