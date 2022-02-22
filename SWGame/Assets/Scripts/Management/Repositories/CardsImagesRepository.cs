using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SWGame.Management.Repositories
{
    static class CardsImagesRepository
    {
        private static List<Sprite> _cards;
        private static List<Sprite> _flippableCards;

        public static List<Sprite> Cards { get => _cards; set => _cards = value; }
        public static List<Sprite> FlippableCards { get => _flippableCards; set => _flippableCards = value; }

        static CardsImagesRepository()
        {
            _cards = new List<Sprite>();
            _flippableCards = new List<Sprite>();
            string path = Environment.CurrentDirectory.Replace(@"\Builds", "") + @"\Assets\Images\Items\Cards";
            string[] links = Directory.GetFiles(path);
            string[] processedLinks = links.Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).ToArray();
            foreach (string link in processedLinks)
            {
                _cards.Add(SpritesLoader.LoadNewSprite(link));
            }
            _flippableCards.Add(_cards[4]);
            _flippableCards.Add(_cards[5]);
        }
    }
}
