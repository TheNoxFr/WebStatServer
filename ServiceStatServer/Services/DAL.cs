using ServiceStatServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace ServiceStatServer.Services
{
    public class DAL : IDAL
    {
        private readonly IDonnees _donnees;
        private readonly ILogger<StatService> _logger;
        private readonly IConfiguration _configuration;

        private readonly SqlConnectionStringBuilder builder;
        private SqlConnection connection;

        public DAL(IDonnees donnees, ILogger<StatService> logger, IConfiguration configuration)
        {
            _donnees = donnees;
            _logger = logger;
            _configuration = configuration;

            builder = new SqlConnectionStringBuilder();
            builder.DataSource = _configuration["MyConfig:SQL:Host"];
            builder.InitialCatalog = _configuration["MyConfig:SQL:Database"];
            builder.UserID = _configuration["MyConfig:SQL:User"];
            builder.Password = _configuration["MyConfig:SQL:Password"];
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
            String sql = "SELECT Id, workbin, agent_id, place_id, media_type, R_AT, A_ALERTE_ECHEANCE FROM interactions where agent_id is not null or place_id is not null";
            _logger.LogInformation("GetIxnData");

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader.GetString(0);
                        string workbin = reader.GetString(1);
                        string agent_id = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        string place_id = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        string media_type = reader.GetString(4);
                        string r_at = reader.IsDBNull(5) ? "" :  reader.GetString(5);
                        string echeance = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        _logger.LogInformation("Read : " + id + " " + workbin + " " + agent_id + " " + place_id + " " + media_type + " " + r_at + " " + echeance);
                        // Ajout interaction
                        _donnees.AjoutInteraction(id, workbin, agent_id, place_id, media_type, r_at, echeance);
                    }
                }
            }
        }
    }
}
