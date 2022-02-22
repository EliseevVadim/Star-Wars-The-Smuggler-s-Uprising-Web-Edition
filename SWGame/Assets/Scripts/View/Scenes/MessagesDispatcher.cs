using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.View.Scenes
{
    public class MessagesDispatcher : MonoBehaviour
    {
        private Queue<Action> _messagesQueue;

        [SerializeField] private float RepeatCheckingMessagesRate;

        private GameObject _connectionErrorMessage;

        private Text _connectionErrorText;

        public GameObject ConnectionErrorMessage
        {
            get { return _connectionErrorMessage; }
            set
            {
                _connectionErrorMessage = value;
                _connectionErrorMessage.transform.SetParent(transform);
            }
        }

        public Text ConnectionErrorText
        {
            get { return _connectionErrorText; }
            set
            {
                _connectionErrorText = value;
                _connectionErrorText.transform.SetParent(_connectionErrorMessage.transform);
            }
        }

        void Start()
        {
            _messagesQueue = new Queue<Action>();
            InvokeRepeating("CheckMessages", 0, RepeatCheckingMessagesRate);
        }

        private void CheckMessages()
        {
            if (_messagesQueue.Count == 0)
                return;
            while (_messagesQueue.Count != 0)
            {
                Action action = _messagesQueue.Dequeue();
                action.Invoke();
            }
        }

        public void AddMessage(Action action)
        {
            _messagesQueue.Enqueue(action);
        }
    }
}
