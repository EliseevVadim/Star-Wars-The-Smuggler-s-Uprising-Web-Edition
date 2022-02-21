﻿using SWGame.Core.Models.Items.Cards;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace SWGame.Core.Services
{
    public class PazaakCardsCreator
    {
        private string _data;

        public PazaakCardsCreator(string data)
        {
            _data = data;
        }

        public Card CreateCard()
        {
            Dictionary<string, object> cardData = JsonConvert.DeserializeObject<Dictionary<string, object>>(_data);
            switch (cardData["TypeName"])
            {
                case "Card":
                    return JsonConvert.DeserializeObject<Card>(_data);
                case "FlippableCard":
                    return JsonConvert.DeserializeObject<FlippableCard>(_data);
                case "GoldCard":
                    return JsonConvert.DeserializeObject<GoldCard>(_data);
                case "ClassicalCard":
                    return JsonConvert.DeserializeObject<ClassicalCard>(_data);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
