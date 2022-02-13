using MySql.Data.MySqlClient;
using SWGame.Core.Management;
using System;

namespace SWGame.Core.Models
{
    public class AuthorizationModel
    {
        private string _nickname;
        private string _password;
        private string _login;

        public AuthorizationModel(string name, string login, string password)
        {
            _nickname = name;
            _password = password;
            _login = login;
        }

        public string Name { get => _nickname; set => _nickname = value; }
        public string Password { get => _password; set => _password = value; }
        public string Login { get => _login; set => _login = value; }

        public Player GetCorrespondingUser()
        {
            using(MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format("SELECT id, Nickname, Login, Password, Credits, Prestige, WisdomPoints, LocationId, AvatarIndex, StoryFinished, LogoutDateTime, ShowTutorial FROM Player " +
                    "WHERE Nickname = @name " +
                    "AND Login = @login " +
                    "AND Password = @password");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", _nickname);
                command.Parameters.AddWithValue("@login", _login);
                command.Parameters.AddWithValue("@password", _password);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Player player = new Player((int)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (long)reader[4],
                        (int)reader[5], (int)reader[6], (int)reader[7], 
                        reader.GetBoolean("StoryFinished"), reader.GetBoolean("ShowTutorial"), (int)reader[8], (DateTime)reader[10]);
                    return player;
                }
                return null;
            }   
        }
    }
}
