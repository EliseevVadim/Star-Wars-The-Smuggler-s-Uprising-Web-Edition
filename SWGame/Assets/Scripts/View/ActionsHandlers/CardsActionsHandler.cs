using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SWGame.Activities.PazaakTools;

namespace SWGame.View.ActionsHandlers
{
    public class CardsActionsHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.clickCount)
            {
                case 1:
                    _image.transform.parent.parent.gameObject.GetComponent<PazaakGame>().SelectCard(int.Parse(_image.tag));
                    break;
                case 2:
                    _image.transform.parent.parent.gameObject.GetComponent<PazaakGame>().AddPlayersCard(int.Parse(_image.tag));
                    break;
            }
        }
    }
}
