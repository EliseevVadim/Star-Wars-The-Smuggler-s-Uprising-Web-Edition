using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SWGame.Core.Management;
using Newtonsoft.Json;

namespace SWGame.Core.Models
{
    public class Player : IDatabaseAgent
    {
        private int _id;
        private string _nickname;
        private string _login;
        private string _password;
        private long _credits;
        private int _prestige;
        private int _wisdomPoints;
        private int _locationId;
        private bool _storyFinished;
        private bool _needToShowTutorial;
        private int _avatarIndex;
        private bool _isOnline;
        private DateTime _logoutDateTime;

        [JsonConstructor]
        public Player(int id, string nickname, string login, string password, long credits, int prestige, int wisdomPoints, int locationId, bool storyFinished, bool needToShowTutorial, int avatarIndex, DateTime logoutDateTime)
        {
            _id = id;
            _nickname = nickname;
            _login = login;
            _password = password;
            _credits = credits;
            _prestige = prestige;
            _wisdomPoints = wisdomPoints;
            _locationId = locationId;
            _storyFinished = storyFinished;
            _needToShowTutorial = needToShowTutorial;
            _avatarIndex = avatarIndex;
            _logoutDateTime = logoutDateTime;
        }

        // constructor 4 players panel display
        public Player(int id, string name, bool isOnline)
        {
            _id = id;
            _nickname = name;
            _isOnline = isOnline;
        }

        public string Nickname { get => _nickname; set => _nickname = value; }
        public string Login { get => _login; set => _login = value; }
        public string Password { get => _password; set => _password = value; }
        public int Prestige { get => _prestige; set => _prestige = value; }
        public int WisdomPoints { get => _wisdomPoints; set => _wisdomPoints = value; }
        public int AvatarIndex { get => _avatarIndex; set => _avatarIndex = value; }
        public bool StoryFinished { get => _storyFinished; set => _storyFinished = value; }
        public bool NeedToShowTutorial { get => _needToShowTutorial; set => _needToShowTutorial = value; }
        public int Id { get => _id; set => _id = value; }
        public int LocationId { get => _locationId; set => _locationId = value; }
        public long Credits { get => _credits; set => _credits = value; }
        public bool IsOnline { get => _isOnline; set => _isOnline = value; }
        public DateTime LogoutDateTime { get => _logoutDateTime; set => _logoutDateTime = value; }

        public async Task AddToDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = string.Format("INSERT INTO Player (Nickname, Login, Password, AvatarIndex, LocationId, Credits) " +
                        "VALUES (@name, @login, @password, @avatarIndex, 1, @credits)");
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", _nickname);
                    command.Parameters.AddWithValue("@login", _login);
                    command.Parameters.AddWithValue("@password", _password);
                    command.Parameters.AddWithValue("@avatarIndex", _avatarIndex);
                    command.Parameters.AddWithValue("@credits", _credits);
                    await command.ExecuteNonQueryAsync();
                    int id = (int)command.LastInsertedId;
                    query = string.Format("INSERT INTO Inventory (PlayersId) VALUES (@id)");
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            } 
        }

        public async Task SaveNewLocationId()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"UPDATE Player SET LocationId = @locationId WHERE id = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", _id);
                command.Parameters.AddWithValue("@locationId", _locationId);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task SaveNewCreditsAmount()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"UPDATE Player SET Credits = @credits WHERE id = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", _id);
                command.Parameters.AddWithValue("@credits", _credits);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task SaveNewTutorialDisplayment()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"UPDATE Player SET ShowTutorial = @needToShow WHERE id = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", _id);
                command.Parameters.AddWithValue("@needToShow", _needToShowTutorial);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task SaveNewPrestigeScore()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"UPDATE Player SET Prestige = @prestige WHERE id = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", _id);
                command.Parameters.AddWithValue("@prestige", _prestige);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task SaveNewWisdomPoints()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"UPDATE Player SET WisdomPoints = @wisdom WHERE id = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", _id);
                command.Parameters.AddWithValue("@wisdom", _wisdomPoints);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateLogoutDateTime()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"UPDATE Player SET LogoutDateTime = @date WHERE id = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", _id);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
