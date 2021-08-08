using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSStatServer.Interfaces;

namespace WSStatServer.Models
{
    public class GRPCSettings : IGRPCSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
    }
}
