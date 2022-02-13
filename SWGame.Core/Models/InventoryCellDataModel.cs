using MySql.Data.MySqlClient;
using SWGame.Core.Management;
using System.Threading.Tasks;

namespace SWGame.Core.Models
{
    public class InventoryCellDataModel
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int ItemId { get; set; }


        public async Task AddToDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format("INSERT INTO InventoryCell (InventoryId, ItemId, Count) VALUES (@invId, @itemId, @count)");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@invId", Id);
                command.Parameters.AddWithValue("@itemId", ItemId);
                command.Parameters.AddWithValue("@count", Count);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateInDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format("UPDATE InventoryCell SET " +
                    "Count = @count " +
                    "WHERE InventoryId = @invId " +
                    "AND ItemId = @itemId");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@invId", Id);
                command.Parameters.AddWithValue("@itemId", ItemId);
                command.Parameters.AddWithValue("@count", Count);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task RemoveFromDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format("DELETE FROM inventorycell " +
                    "WHERE InventoryId = @invId " +
                    "AND ItemId = @itemId");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@invId", Id);
                command.Parameters.AddWithValue("@itemId", ItemId);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
