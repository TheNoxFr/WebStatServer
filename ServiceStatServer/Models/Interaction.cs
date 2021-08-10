using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer.Models
{
    public class Interaction
    {
        public string IxnId { get; set; }
        public string AgentId { get; set; }
        public string Workbin { get; set; }
        public string Media { get; set; }
    }
}
