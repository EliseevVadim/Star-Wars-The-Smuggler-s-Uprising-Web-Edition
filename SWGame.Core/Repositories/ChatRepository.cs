using MySql.Data.MySqlClient;
using SWGame.Core.Management;
using SWGame.Core.Models;
using System;
using System.Collections.Generic;

namespace SWGame.Core.Repositories
{
    public class ChatRepository : IRepository<Chat>
    {
        public List<Chat> LoadAll()
        {
            throw new NotImplementedException();
        }

        public Chat LoadById(int id)
        {
            Chat chat = new Chat(id);
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"SELECT ChatId, SendTime, Nickname, Content
                                FROM chatmessage INNER JOIN player ON chatmessage.AuthorId = player.id
                                WHERE ChatId = @id
                                ORDER BY chatmessage.id DESC
                                LIMIT 100");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    chat.Messages.Add(new ChatMessage()
                    {
                        ChatId = (int)reader[0],
                        SendTimeLine = (string)reader[1],
                        AuthorName = (string)reader[2],
                        Message = (string)reader[3]
                    });
                }
            }
            return chat;
        }
    }
}
