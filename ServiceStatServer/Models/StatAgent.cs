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
        public int NbEmail { get; set; }
        public int NbTache { get; set; }

        public StatAgent()
        {
            NbEmail = 0;
            NbTache = 0;
        }
    }
}
