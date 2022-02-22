using MySql.Data.MySqlClient;
using SWGame.Core.Management;
using SWGame.Core.Models;
using System.Collections.Generic;

namespace SWGame.Core.Repositories
{
    public class LocationsRepository : IRepository<Location>
    {
        public List<Location> LoadAll()
        {
            List<Location> locations = new List<Location>();
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = "SELECT id, Name, PlanetId FROM Location";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    locations.Add(new Location
                    {
                        Id = (int)reader[0],
                        Name = (string)reader[1],
                        PlanetId = (int)reader[2]
                    });
                }
            }
            return locations;
        }

        public Location LoadById(int id)
        {
            Location location = null;
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"SELECT id, Name, PlanetId FROM Location WHERE id = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    location = new Location
                    {
                        Id = (int)reader[0],
                        Name = (string)reader[1],
                        PlanetId = (int)reader[2]
                    };
                }
            }
            return location;
        }
        public Dictionary<int, string> LoadLocationsNames()
        {
            Dictionary<int, string> names = new Dictionary<int, string>();
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = "SELECT id, Name FROM Location";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    names.Add((int)reader[0], (string)reader[1]);
                }
            }
            return names;
        }
    }
}
