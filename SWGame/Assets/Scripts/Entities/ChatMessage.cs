using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGame.Entities
{
    public class ChatMessage
    {
        private int _authorsId;
        private string _sendTimeLine;
        private string _message;
        private int _chatId;
        private string _authorName;

        public int AuthorsId { get => _authorsId; set => _authorsId = value; }
        public string SendTimeLine { get => _sendTimeLine; set => _sendTimeLine = value; }
        public string Message { get => _message; set => _message = value; }
        public int ChatId { get => _chatId; set => _chatId = value; }
        public string AuthorName { get => _authorName; set => _authorName = value; }
    }
}
