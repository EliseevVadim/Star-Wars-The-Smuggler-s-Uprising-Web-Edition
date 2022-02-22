using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SWGame.Management.Repositories
{
    static class LootItemsIconsRepository
    {
        private static List<Sprite> _itemsSprites;

        public static List<Sprite> ItemsSprites { get => _itemsSprites; set => _itemsSprites = value; }

        static LootItemsIconsRepository()
        {
            _itemsSprites = new List<Sprite>();
            string path = Environment.CurrentDirectory.Replace(@"\Builds", "") + @"\Assets\Images\Items\LootItems";
            string[] links = Directory.GetFiles(path);
            string[] processedLinks = links.Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).ToArray();
            foreach (string link in processedLinks)
            {
                _itemsSprites.Add(SpritesLoader.LoadNewSprite(link));
            }
        }
    }
}
