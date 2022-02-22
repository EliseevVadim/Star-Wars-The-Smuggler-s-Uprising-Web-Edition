using SWGame.Entities;
using UnityEngine;

namespace SWGame.View.Presenters
{
    public class ChatVisualizator : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private MessagePresenter _messageTemplate;

        public void Render(Chat chat)
        {
            foreach (Transform message in _container)
            {
                Destroy(message.gameObject);
            }
            chat.Messages.ForEach(message =>
            {
                var messageView = Instantiate(_messageTemplate, _container);
                messageView.Visualize(message);
            });
        }
    }
}
