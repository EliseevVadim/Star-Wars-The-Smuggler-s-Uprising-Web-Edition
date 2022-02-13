using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWGame.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.View.Presenters
{
    public class MessagePresenter : MonoBehaviour
    {
        [SerializeField] private Text _timeField;
        [SerializeField] private Text _messageField;
        [SerializeField] private Text _authorField; 

        public void Visualize(ChatMessage message)
        {
            _timeField.text = message.SendTimeLine;
            _authorField.text = message.AuthorName;
            _messageField.text = message.Message;
        }
    }
}
