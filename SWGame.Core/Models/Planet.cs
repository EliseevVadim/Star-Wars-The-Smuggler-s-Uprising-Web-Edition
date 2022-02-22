using MySql.Data.MySqlClient;
using SWGame.Core.Management;
using System.Threading.Tasks;

namespace SWGame.Core.Models
{
    public class Planet
    {
        private int _id;
        private string _name;
        private string _description;
        private long _treasury;
        private int _travellCost;
        private int _iconIndex;

        //public Planet(int id, string name, string description, int treasury, int travelCost, int iconIndex)
        //{
        //    _id = id;
        //    _name = name;
        //    _description = description;
        //    _treasury = treasury;
        //    _travelCost = travelCost;
        //    _iconIndex = iconIndex;
        //}

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }
        public long Treasury { get => _treasury; set => _treasury = value; }
        public int TravellCost { get => _travellCost; set => _travellCost = value; }
        public int IconIndex { get => _iconIndex; set => _iconIndex = value; }

        public async Task SaveNewTreasury()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"UPDATE Planet SET Treasury = @treasury WHERE id = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", _id);
                command.Parameters.AddWithValue("@treasury", _treasury);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
