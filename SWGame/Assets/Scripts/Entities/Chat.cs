﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGame.Entities
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
