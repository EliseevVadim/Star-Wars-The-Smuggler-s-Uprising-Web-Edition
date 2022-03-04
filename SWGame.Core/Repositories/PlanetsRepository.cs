using MySql.Data.MySqlClient;
using SWGame.Core.Management;
using SWGame.Core.Models;
using System.Collections.Generic;

namespace SWGame.Core.Repositories
{
    public class PlanetsRepository : IRepository<Planet>
    {
        public List<Planet> LoadAll()
        {
            List<Planet> planets = new List<Planet>();
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = "SELECT id, Name, Description, TravellCost, Treasury, IconIndex FROM planet";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    planets.Add(new Planet
                    {
                        Id = (int)reader[0],
                        Name = (string)reader[1],
                        Description = (string)reader[2],
                        TravellCost = (int)reader[3],
                        Treasury = (long)reader[4],
                        IconIndex = (int)reader[5]
                    });
                }
            }
            return planets;
        }

        public Planet LoadById(int id)
        {
            Planet planet = null;
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"SELECT id, Name, Description, TravellCost, Treasury, IconIndex FROM planet WHERE id = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    planet = new Planet
                    {
                        Id = (int)reader[0],
                        Name = (string)reader[1],
                        Description = (string)reader[2],
                        TravellCost = (int)reader[3],
                        Treasury = (int)reader[4],
                        IconIndex = (int)reader[5]
                    };
                }
            }
            return planet;
        }
    }
}
