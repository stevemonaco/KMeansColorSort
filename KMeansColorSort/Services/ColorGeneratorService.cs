using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using KMeansColorSort.Models;

namespace KMeansColorSort.Services
{
    interface IColorGeneratorService
    {
        IEnumerable<ColorModel> CreateSystemDrawingColors();
        IEnumerable<ColorModel> CreateRandomColors(int colorCount, Random random);
    }

    class ColorGeneratorService : IColorGeneratorService
    {
        public IEnumerable<ColorModel> CreateSystemDrawingColors()
        {
            return Enum.GetValues(enumType: typeof(KnownColor))
                .Cast<KnownColor>()
                .Select(x => Color.FromKnownColor(x))
                .Where(x => !x.IsSystemColor && x.A > 0)
                .Select(x => new ColorModel(x.R, x.G, x.B, x.A, x.GetHue(), x.GetSaturation(), x.GetBrightness()));
        }

        public IEnumerable<ColorModel> CreateRandomColors(int colorCount, Random random)
        {
            return Enumerable.Range(1, colorCount)
                .Select(x => Color.FromArgb(255, random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)))
                .Select(x => new ColorModel(x.R, x.G, x.B, x.A, x.GetHue(), x.GetSaturation(), x.GetBrightness()));
        }
    }
}
