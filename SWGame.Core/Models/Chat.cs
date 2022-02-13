using System.Collections.Generic;

namespace SWGame.Core.Models
{
    public class Chat
    {
        private int _locationId;
        private List<ChatMessage> _messages;

        public Chat(int locationId)
        {
            _locationId = locationId;
            _messages = new List<ChatMessage>();
        }

        public int LocationId { get => _locationId; set => _locationId = value; }
        public List<ChatMessage> Messages { get => _messages; set => _messages = value; }
    }
}
