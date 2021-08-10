using ServiceStatServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ServiceStatServer.Services
{
    public class DAL : IDAL
    {
        private readonly IDonnees _donnees;
        private readonly ILogger<StatService> _logger;

        private SqlConnectionStringBuilder builder;
        private SqlConnection connection;

        public DAL(IDonnees donnees, ILogger<StatService> logger)
        {
            _donnees = donnees;
            _logger = logger;

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
            String sql = "SELECT Id, workbin, agent_id, media_type FROM MyInteractions";
            _logger.LogInformation("GetIxnData");

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _logger.LogInformation("Read : " + reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3));
                        // Ajout interaction
                        _donnees.AjoutInteraction(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                    }
                }
            }
        }
    }
}
