using SWGame.Core.Models;
using SWGame.Core.Management;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace SWGame.Core.Repositories
{
    public class ShopsRepository : IRepository<Shop>
    {
        public List<Shop> LoadAll()
        {
            throw new NotImplementedException();
        }

        public Shop LoadById(int locationId)
        {
            Shop shop = null;
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format("SELECT shop.id, shop.Name, shop.Revenue " +
                    "FROM shop INNER JOIN location ON shop.LocationId = location.id " +
                    "WHERE shop.LocationId = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", locationId);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        shop = new Shop
                        {
                            Id = (int)reader[0],
                            Name = (string)reader[1],
                            Revenue = (int)reader[2],
                            LocationId = locationId
                        };
                    }
                    catch
                    {

                    }
                }
            }
            return shop;
        }
    }
}
