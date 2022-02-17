using SWGame.Activities;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.View.ActionsHandlers
{
    public class KeyResponsiveInputField : MonoBehaviour
    {
        [SerializeField] InputField _responsiveField;

        void Start()
        {
            _responsiveField.onEndEdit.AddListener(delegate { SendMessageViaPressingEnterButton(); });
        }

        private void SendMessageViaPressingEnterButton()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {         
                GetComponentInParent<ChatMessageSender>().Send();
            }
        }
    }
}