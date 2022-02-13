using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SWGame.Management.Repositories
{
    static class QuestItemsIconsRepository
    {
        private static List<Sprite> _icons;

        public static List<Sprite> Icons { get => _icons; set => _icons = value; }

        static QuestItemsIconsRepository()
        {
            _icons = new List<Sprite>();
            string path = Environment.CurrentDirectory.Replace(@"\Builds", "") + @"\Assets\Images\Items\QuestItems";
            string[] links = Directory.GetFiles(path);
            string[] processedLinks = links.Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).ToArray();
            foreach (string link in processedLinks)
            {
                _icons.Add(SpritesLoader.LoadNewSprite(link));
            }
        }
    }
}
