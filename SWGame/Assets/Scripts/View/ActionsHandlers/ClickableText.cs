﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SWGame.View.ActionsHandlers
{
    public class ClickableText : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private UnityEvent _onClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick.Invoke();
        }
    }
}
