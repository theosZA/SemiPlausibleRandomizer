using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemiPlausibleRandomizer.Graphics
{
    public static class BitmapHelper
    {
        /// <summary>
        /// Breaks the bitmap up into areas of pixels that are the same colour. For each such area, all adjacent areas are found.
        /// </summary>
        /// <param name="bmp">A 24 bpp bitmap image.</param>
        /// <returns>A mapping from each pixel colour in the image to a collection of the pixel colours of adjacent areas.</returns>
        public static IDictionary<Color, ISet<Color>> CalculateAreaAdjacency(this Bitmap bmp)
        {
            if (bmp.PixelFormat != PixelFormat.Format24bppRgb)
            {
                throw new ArgumentException($"Unhandled bitmap pixel format {bmp.PixelFormat.ToString()}");
            }

            var mapping = new Dictionary<Color, ISet<Color>>();
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);

            unsafe
            {
                byte* originPtr;
                byte* startLinePtr;
                var comparePtrs = new byte*[4];
                var minAllowedPtrs = new byte*[4];
                var maxAllowedPtrs = new byte*[4];
                minAllowedPtrs[0] = (byte*)bmpData.Scan0;
                minAllowedPtrs[3] = minAllowedPtrs[0];
                maxAllowedPtrs[0] = (byte*)bmpData.Scan0 + bmp.Height * bmpData.Stride;
                maxAllowedPtrs[3] = maxAllowedPtrs[0];
                for (int y = 0; y < bmp.Height; ++y)
                {
                    startLinePtr = (byte*)bmpData.Scan0 + bmpData.Stride * y;
                    originPtr = startLinePtr;
                    comparePtrs[0] = originPtr - bmpData.Stride;
                    comparePtrs[1] = originPtr - 1;
                    comparePtrs[2] = originPtr + 1;
                    comparePtrs[3] = originPtr + bmpData.Stride;
                    minAllowedPtrs[1] = startLinePtr;
                    minAllowedPtrs[2] = minAllowedPtrs[2];
                    maxAllowedPtrs[1] = startLinePtr + bmp.Width * 3;
                    maxAllowedPtrs[2] = maxAllowedPtrs[2];

                    for (int x = 0; x < bmp.Width; ++x)
                    {
                        var originColour = Color.FromArgb(*(originPtr + 2), *(originPtr + 1), *originPtr);
                        for (int i = 0; i < comparePtrs.Length; ++i)
                        {
                            if (comparePtrs[i] >= minAllowedPtrs[i] && comparePtrs[i] < maxAllowedPtrs[i])
                            {
                                var compareColour = Color.FromArgb(*(comparePtrs[i] + 2), *(comparePtrs[i] + 1), *comparePtrs[i]);
                                if (originColour != compareColour)
                                {   // Found an adjacent area. Add it to the mapping.
                                    if (mapping.TryGetValue(originColour, out var adjacentColours))
                                    {
                                        adjacentColours.Add(compareColour);
                                    }
                                    else
                                    {
                                        mapping.Add(originColour, new HashSet<Color>());
                                        mapping[originColour].Add(compareColour);
                                    }
                                }
                            }
                        }

                        originPtr += 3;
                        for (int i = 0; i < comparePtrs.Length; ++i)
                        {
                            comparePtrs[i] += 3;
                        }
                    }
                }
            }

            bmp.UnlockBits(bmpData);
            return mapping;
        }
    }
}
