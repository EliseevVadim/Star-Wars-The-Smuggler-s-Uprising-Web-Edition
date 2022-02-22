using SWGame.Activities.PazaakTools;
using SWGame.Activities.PazaakTools.OnlinePazaak;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SWGame.View.ActionsHandlers
{
    public class CardsActionsHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnlinePazaakGame onlinePazaak = (OnlinePazaakGame)Resources.FindObjectsOfTypeAll(typeof(OnlinePazaakGame)).ToArray()[0];
            switch (eventData.clickCount)
            {
                case 1:
                    if (onlinePazaak.gameObject.activeSelf)
                    {
                        _image.transform.parent.parent.gameObject.GetComponent<OnlinePazaakGame>().SelectCard(int.Parse(_image.tag));
                    }
                    else
                    {
                        _image.transform.parent.parent.gameObject.GetComponent<PazaakGame>().SelectCard(int.Parse(_image.tag));
                    }
                    break;
                case 2:
                    if (onlinePazaak.gameObject.activeSelf)
                    {
                        _image.transform.parent.parent.gameObject.GetComponent<OnlinePazaakGame>().AddPlayersCard(int.Parse(_image.tag));
                    }
                    else
                    {
                        _image.transform.parent.parent.gameObject.GetComponent<PazaakGame>().AddPlayersCard(int.Parse(_image.tag));
                    }
                    break;
            }
        }
    }
}
