using System.IO;
using UnityEngine;

namespace SWGame.Management
{
    static class SpritesLoader
    {
        public static Sprite LoadNewSprite(string filePath, float PixelsPerUnit = 100.0f)
        {
            Sprite newSprite;
            Texture2D spriteTexture = LoadTexture(filePath);
            newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), PixelsPerUnit);
            return newSprite;
        }

        public static Texture2D LoadTexture(string filePath)
        {
            Texture2D texture;
            byte[] fileData;
            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                texture = new Texture2D(2, 2);
                if (texture.LoadImage(fileData))
                {
                    return texture;
                }
            }
            return null;
        }
    }
}
