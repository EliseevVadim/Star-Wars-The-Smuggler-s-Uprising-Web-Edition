using Newtonsoft.Json;
using SWGame.Enums;
using SWGame.Management.Repositories;
using SWGame.Activities.PazaakTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SWGame.View.Scenes;

namespace SWGame.Entities.Items.Cards
{
    public class GoldCard : Card
    {
        private GoldCardType _type;
        private int _index;
        private int[] _possibleValues;

        [JsonConstructor]
        public GoldCard(int id, string name, GoldCardType type, int value, int index) : base(id, name, value)
        {
            _index = index;
            _image = CardsImagesRepository.Cards[1];
            _type = type;
            switch (_type)
            {
                case GoldCardType.PlusMinusOneOrTwo:
                    _possibleValues = new int[] { 1, -1, 2, -2 };
                    _value = _possibleValues[_index];
                    break;
                case GoldCardType.TCard:
                    _possibleValues = new int[] { 1, -1 };
                    _value = _possibleValues[_index];
                    break;
            }
            GenerateLineFromValue();
        }

        public GoldCardType Type { get => _type; set => _type = value; }
        public int Index { get => _index; set => _index = value; }
        public int[] PossibleValues { get => _possibleValues; set => _possibleValues = value; }

        public override void AddToDeck(Deck deck)
        {
            int index = deck.CurrentIndex;
            deck.Cards[index] = this;
            deck.DeckView[index].sprite = _image;
            deck.DeckView[index].color = Color.white;
            deck.CardsValues[index].text = _valueInLine;
            switch (_type)
            {
                case GoldCardType.DCard:
                    try
                    {
                        deck.Sum += deck.Cards[index - 1].Value;
                    }
                    catch { }
                    break;
                case GoldCardType.PlusMinusOneOrTwo:
                    deck.Sum += _value;
                    break;
                case GoldCardType.TCard:
                    deck.Sum += _value;
                    deck.HasATiebreaker = true;
                    break;
                case GoldCardType.ThreeAndSix:
                    for (int i = 0; i < deck.Cards.Length; i++)
                    {
                        try
                        {
                            if ((deck.Cards[i].Value == 3 || deck.Cards[i].Value == 6) &&
                                !(deck.Cards[i] is ClassicalCard || deck.Cards[i] is FlippableCard))
                            {
                                deck.Sum -= 2 * deck.Cards[i].Value;
                                deck.DeckView[i].sprite = CardsImagesRepository.Cards[3];
                                deck.CardsValues[i].text = "-" + deck.CardsValues[i].text;
                            }
                        }
                        catch { }
                    }
                    break;
                case GoldCardType.TwoAndFour:
                    for (int i = 0; i < deck.Cards.Length; i++)
                    {
                        try
                        {
                            if ((deck.Cards[i].Value == 2 || deck.Cards[i].Value == 4) &&
                                !(deck.Cards[i] is ClassicalCard || deck.Cards[i] is FlippableCard))
                            {
                                deck.Sum -= 2 * deck.Cards[i].Value;
                                deck.DeckView[i].sprite = CardsImagesRepository.Cards[3];
                                deck.CardsValues[i].text = "-" + deck.CardsValues[i].text;
                            }
                        }
                        catch { }
                    }
                    break;
            }
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
                switch (_type)
                {
                    case GoldCardType.DCard:
                        try
                        {
                            deck.Sum += deck.Cards[index - 1].Value;
                        }
                        catch { }
                        break;
                    case GoldCardType.PlusMinusOneOrTwo:
                        deck.Sum += _value;
                        break;
                    case GoldCardType.TCard:
                        deck.Sum += _value;
                        deck.HasATiebreaker = true;
                        break;
                    case GoldCardType.ThreeAndSix:
                        for (int i = 0; i < deck.Cards.Length; i++)
                        {
                            try
                            {
                                if ((deck.Cards[i].Value == 3 || deck.Cards[i].Value == 6) &&
                                    !(deck.Cards[i] is ClassicalCard || deck.Cards[i] is FlippableCard))
                                {
                                    deck.Sum -= 2 * deck.Cards[i].Value;
                                    deck.DeckView[i].sprite = CardsImagesRepository.Cards[3];
                                    deck.CardsValues[i].text = "-" + deck.CardsValues[i].text;
                                }
                            }
                            catch { }
                        }
                        break;
                    case GoldCardType.TwoAndFour:
                        for (int i = 0; i < deck.Cards.Length; i++)
                        {
                            try
                            {
                                if ((deck.Cards[i].Value == 2 || deck.Cards[i].Value == 4) &&
                                    !(deck.Cards[i] is ClassicalCard || deck.Cards[i] is FlippableCard))
                                {
                                    deck.Sum -= 2 * deck.Cards[i].Value;
                                    deck.DeckView[i].sprite = CardsImagesRepository.Cards[3];
                                    deck.CardsValues[i].text = "-" + deck.CardsValues[i].text;
                                }
                            }
                            catch { }
                        }
                        break;
                }
                deck.CurrentIndex++;
            }));
        }

        public override bool Equals(object obj)
        {
            return obj is GoldCard card &&
                   _id == card._id &&
                   _descriprion == card._descriprion &&
                   EqualityComparer<Sprite>.Default.Equals(_image, card._image) &&
                   _salePrice == card._salePrice &&
                   _name == card._name &&
                   _type == card._type;
        }

        public override int GetHashCode()
        {
            int hashCode = 1482191999;
            hashCode = hashCode * -1521134295 + _id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_descriprion);
            hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(_image);
            hashCode = hashCode * -1521134295 + _salePrice.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_name);
            hashCode = hashCode * -1521134295 + _type.GetHashCode();
            return hashCode;
        }

        public override void GenerateLineFromValue()
        {
            switch (_type)
            {
                case GoldCardType.DCard:
                    _valueInLine = "D";
                    break;
                case GoldCardType.TCard:
                    _valueInLine = "+/-1T";
                    break;
                case GoldCardType.ThreeAndSix:
                    _valueInLine = "3&6";
                    break;
                case GoldCardType.TwoAndFour:
                    _valueInLine = "2&4";
                    break;
                case GoldCardType.PlusMinusOneOrTwo:
                    _valueInLine = "+/- 1or2";
                    break;
            }
        }
    }
}
