using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SWGame.View.ActionsHandlers
{
    public class ButtonActionsHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private Text _infoLabel;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _infoLabel.text = _button.tag;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _infoLabel.text = string.Empty;
        }
    }
}
