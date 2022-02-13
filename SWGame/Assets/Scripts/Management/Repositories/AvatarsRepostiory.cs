using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace SWGame.Management.Repositories
{
    static class AvatarsRepository
    {
        private static List<Sprite> _avatars;

        public static List<Sprite> Avatars => _avatars;

        static AvatarsRepository()
        {
            _avatars = new List<Sprite>();
            string path = Environment.CurrentDirectory.Replace(@"\Builds", "") + @"\Assets\Images\Faces";
            string[] links = Directory.GetFiles(path);
            string[] processedLinks = links.Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).ToArray();
            foreach (string link in processedLinks)
            {
                _avatars.Add(SpritesLoader.LoadNewSprite(link));
            }
        }
    }
}
