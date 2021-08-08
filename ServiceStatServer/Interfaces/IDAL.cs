using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStatServer.Interfaces
{
    public interface IDAL
    {
        bool Connexion();
        void Deconnexion();
        void GetIxnData();

    }
}
