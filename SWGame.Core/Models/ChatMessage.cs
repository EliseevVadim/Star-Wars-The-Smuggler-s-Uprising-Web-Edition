using MySql.Data.MySqlClient;
using SWGame.Core.Management;
using System.Threading.Tasks;

namespace SWGame.Core.Models
{
    public class ChatMessage : IDatabaseAgent
    {
        private int _authorsId;
        private string _sendTimeLine;
        private string _message;
        private int _chatId;
        private string _authorName;

        public string SendTimeLine { get => _sendTimeLine; set => _sendTimeLine = value; }
        public string Message { get => _message; set => _message = value; }
        public int ChatId { get => _chatId; set => _chatId = value; }
        public string AuthorName { get => _authorName; set => _authorName = value; }
        public int AuthorsId { get => _authorsId; set => _authorsId = value; }

        public async Task AddToDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"INSERT INTO chatmessage (ChatId, SendTime, AuthorId, Content)
                                                VALUES (@chatId, @send, @author, @content)");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@chatId", _chatId);
                command.Parameters.AddWithValue("@send", _sendTimeLine);
                command.Parameters.AddWithValue("@author", _authorsId);
                command.Parameters.AddWithValue("@content", _message);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
