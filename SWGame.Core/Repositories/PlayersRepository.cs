using MySql.Data.MySqlClient;
using SWGame.Core.Management;
using SWGame.Core.Models;
using SWGame.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace SWGame.Core.Repositories
{
    public class PlayersRepository : IRepository<Player>
    {
        public List<Player> LoadAll()
        {
            throw new System.NotImplementedException();
        }

        public Player LoadById(int id)
        {
            Player player = null;
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"SELECT id, Nickname, Login, Password, Credits, Prestige, WisdomPoints, LocationId, AvatarIndex, StoryFinished, LogoutDateTime, ShowTutorial FROM player WHERE id = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    player = new Player((int)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (long)reader[4],
                        (int)reader[5], (int)reader[6], (int)reader[7],
                        reader.GetBoolean("StoryFinished"), reader.GetBoolean("ShowTutorial"), (int)reader[8], (DateTime)reader[10]);
                    return player;
                }
            }
            return player;
        }

        public PlayerViewModel LoadPlayersInfoById(int id)
        {
            PlayerViewModel playerViewModel = null;
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"SELECT Nickname, Prestige, WisdomPoints, AvatarIndex, StoryFinished, planet.Name, location.Name
                                        FROM player INNER JOIN location ON player.LocationId = location.id
                                        INNER JOIN planet ON planet.id = location.Planetid
                                        WHERE player.id = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    playerViewModel = new PlayerViewModel
                    {
                        Name = (string)reader[0],
                        Prestige = (int)reader[1],
                        WisdomPoints = (int)reader[2],
                        AvatarIndex = (int)reader[3],
                        StoryFinished = reader.GetBoolean("StoryFinished"),
                        PlanetName = (string)reader[5],
                        LocationName = (string)reader[6],
                        IsOnline = null
                    };
                }
                return playerViewModel;
            }
        }

        public List<Player> LoadByLocation(int locationId)
        {
            List<Player> players = new List<Player>();
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"SELECT id, Nickname FROM player WHERE LocationId = @id");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", locationId);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    players.Add(new Player((int)reader[0], (string)reader[1], OnlineUsers.IsUserOnline((int)reader[0])));
                }
            }
            return players;
        }
    }
}
