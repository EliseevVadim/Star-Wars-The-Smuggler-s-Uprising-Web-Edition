using Newtonsoft.Json;
using SWGame.Activities.PazaakTools;
using SWGame.Management.Repositories;
using SWGame.View.Scenes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SWGame.Entities.Items.Cards
{
    public class ClassicalCard : Card
    {
        [JsonConstructor]
        public ClassicalCard(int id, string name, int value) : base(id, name, value)
        {
            if (_value > 0)
            {
                _image = CardsImagesRepository.Cards[2];
            }
            else
            {
                _image = CardsImagesRepository.Cards[3];
            }
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

        public override bool Equals(object obj)
        {
            return obj is ClassicalCard card &&
                   _id == card._id &&
                   _descriprion == card._descriprion &&
                   EqualityComparer<Sprite>.Default.Equals(_image, card._image) &&
                   _salePrice == card._salePrice &&
                   _name == card._name &&
                   _value == card._value;
        }

        public override int GetHashCode()
        {
            int hashCode = -1542751474;
            hashCode = hashCode * -1521134295 + _id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_descriprion);
            hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(_image);
            hashCode = hashCode * -1521134295 + _salePrice.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_name);
            hashCode = hashCode * -1521134295 + _value.GetHashCode();
            return hashCode;
        }

        public override void GenerateLineFromValue()
        {
            _valueInLine = _value > 0 ? $"+{_value}" : _value.ToString();
        }
    }
}
