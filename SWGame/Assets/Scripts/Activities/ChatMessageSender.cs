using SWGame.Entities;
using SWGame.GlobalConfigurations;
using SWGame.Management;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.Activities
{
    public class ChatMessageSender : MonoBehaviour
    {
        [SerializeField] private InputField _messageField;
        private ClientManager _clientManager;

        private void Start()
        {
            _clientManager = FindObjectOfType<ClientManager>();
        }

        public async void Send()
        {
            string content = _messageField.text;
            if (!String.IsNullOrWhiteSpace(content))
            {
                ChatMessage message = new ChatMessage()
                {
                    AuthorsId = CurrentPlayer.Player.Id,
                    AuthorName = CurrentPlayer.Player.Nickname,
                    ChatId = CurrentPlayer.Player.LocationId,
                    SendTimeLine = GetTimeLine(),
                    Message = content
                };
                _messageField.text = string.Empty;
                await _clientManager.SendMessage(message);
            }
        }

        private string GetTimeLine()
        {
            DateTime sendTime = DateTime.Now;
            return $"[{sendTime.Hour.ToString("00")}:{sendTime.Minute.ToString("00")}]";
        }
    }
}
