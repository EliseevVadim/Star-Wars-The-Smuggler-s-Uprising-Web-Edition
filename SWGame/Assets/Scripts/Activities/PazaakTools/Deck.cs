using SWGame.Entities.Items.Cards;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.Activities.PazaakTools
{
    public class Deck : MonoBehaviour
    {
        [SerializeField] private Image[] _deckView;
        [SerializeField] private Text[] _cardsValues;

        private int _sum;
        private int _currentIndex;
        private bool _hasATiebreaker;
        private Card[] _cards;

        public Image[] DeckView { get => _deckView; set => _deckView = value; }
        public Text[] CardsValues { get => _cardsValues; set => _cardsValues = value; }
        public Card[] Cards { get => _cards; set => _cards = value; }
        public int Sum { get => _sum; set => _sum = value; }
        public int CurrentIndex { get => _currentIndex; set => _currentIndex = value; }
        public bool HasATiebreaker { get => _hasATiebreaker; set => _hasATiebreaker = value; }

        private void Awake()
        {
            _cards = new Card[9];
            _sum = 0;
            _currentIndex = 0;
            _hasATiebreaker = false;
        }

        public void Clear()
        {
            if (_cards == null)
            {
                _cards = new Card[9];
            }
            else
            {
                Array.Clear(_cards, 0, _cards.Length);
            }
            _currentIndex = 0;
            _sum = 0;
            _hasATiebreaker = false;
            for (int i = 0; i < _deckView.Length; i++)
            {
                _deckView[i].color = new Color(0.3215686f, 0.3215686f, 0.3215686f);
                _deckView[i].sprite = null;
                _cardsValues[i].text = string.Empty;
            }
        }
    }
}
