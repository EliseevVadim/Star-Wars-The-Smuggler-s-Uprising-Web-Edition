using Newtonsoft.Json;
using SWGame.Activities.PazaakTools;
using SWGame.Management.Repositories;
using SWGame.View.Scenes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SWGame.Entities.Items.Cards
{
    public class FlippableCard : Card, ICloneable
    {
        [JsonConstructor]
        public FlippableCard(int id, string name, int value) : base(id, name, value)
        {
            _image = CardsImagesRepository.FlippableCards[0];
            GenerateLineFromValue();
        }

        public override void AddToDeck(Deck deck)
        {
            int index = deck.CurrentIndex;
            deck.Cards[index] = this;
            deck.DeckView[index].sprite = _image;
            deck.DeckView[index].color = Color.white;
            deck.CardsValues[index].text = _valueInLine;
            deck.Sum += _value;
            deck.CurrentIndex++;
        }

        public override void AddServerCardToDeck(Deck deck, MessagesDispatcher dispatcher)
        {
            dispatcher.AddMessage(new Action(() =>
            {
                int index = deck.CurrentIndex;
                deck.Cards[index] = this;
                deck.DeckView[index].sprite = _image;
                deck.DeckView[index].color = Color.white;
                deck.CardsValues[index].text = _valueInLine;
                deck.Sum += _value;
                deck.CurrentIndex++;
            }));
        }

        public override void GenerateLineFromValue()
        {
            _valueInLine = $"+/-{_value}";
        }
        public void Flip()
        {
            _value = -_value;
            if (_value > 0)
            {
                _image = CardsImagesRepository.FlippableCards[0];
                _valueInLine = $"+{_value}";
            }
            else
            {
                _image = CardsImagesRepository.FlippableCards[1];
                _valueInLine = _value.ToString();
            }
        }

        public override bool Equals(object obj)
        {
            return obj is FlippableCard card &&
                   _id == card._id &&
                   _descriprion == card._descriprion &&
                   EqualityComparer<Sprite>.Default.Equals(_image, card._image) &&
                   _salePrice == card._salePrice &&
                   _name == card._name;
        }

        public override int GetHashCode()
        {
            int hashCode = -1542751474;
            hashCode = hashCode * -1521134295 + _id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_descriprion);
            hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(_image);
            hashCode = hashCode * -1521134295 + _salePrice.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_name);
            return hashCode;
        }

        public new object Clone() => MemberwiseClone();
    }
}
