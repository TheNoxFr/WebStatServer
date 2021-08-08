using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSStatServer.Interfaces
{
    public interface IGRPCSettings
    {
        string Host { get; set; }
        string Port { get; set; }
    }
}
