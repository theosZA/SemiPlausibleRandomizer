using System;
using System.Drawing;
using System.IO;

namespace SemiPlausibleRandomizer.Graphics
{
    internal class TgaImage
    {
        public int Width => pixels.GetLength(1);
        public int Height => pixels.GetLength(0);

        public TgaImage(int width, int height, Color backgroundColour)
        {
            pixels = new Color[height, width];
            for (int r = 0; r < height; ++r)
                for (int c = 0; c < width; ++c)
                    pixels[r, c] = backgroundColour;
        }

        public void Save(string fileName)
        {
            using (var writer = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
            {
                writer.Write((byte)0);  // Image ID length
                writer.Write((byte)0);  // Color map type = none
                writer.Write((byte)2); // Image type = uncompressed true-color image
                writer.Write(new byte[5]);  // Color map specification unused
                writer.Write((UInt16)0);    // X-origin
                writer.Write((UInt16)0);    // Y-origin
                writer.Write((UInt16)Width);
                writer.Write((UInt16)Height);
                writer.Write((UInt16)24);   // bits per pixel
                writer.Write((byte)0);  // Image descriptor = default
                for (int r = 0; r < Height; ++r)
                    for (int c = 0; c < Width; ++c)
                    {
                        writer.Write(pixels[r, c].G);
                        writer.Write(pixels[r, c].R);
                        writer.Write(pixels[r, c].B);
                    }
            }
        }

        Color[,] pixels = null;
    }
}
