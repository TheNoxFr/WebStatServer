using ServiceStatServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ServiceStatServer.Services
{
    public class DAL : IDAL
    {
        private readonly IDonnees _donnees;

        private SqlConnectionStringBuilder builder;
        private SqlConnection connection;

        public DAL(IDonnees donnees)
        {
            _donnees = donnees;

            builder = new SqlConnectionStringBuilder();
            builder.DataSource = "genserv";
            builder.UserID = "genesys";
            builder.Password = "Euroviva1";
            builder.InitialCatalog = "genixn";
        }

        public bool Connexion()
        {
            connection = new SqlConnection(builder.ConnectionString);

            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void Deconnexion()
        {
            try
            {
                connection.Close();
            }
            catch (Exception)
            {
            }
        }

        public void GetIxnData()
        {
            String sql = "SELECT Id, workbin, agent_id FROM MyInteractions";

            using(SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Ajout interaction
                        _donnees.AjoutInteraction(reader.GetString(0), reader.GetString(1), reader.GetString(2));
                    }
                }
            }
        }
    }
}
