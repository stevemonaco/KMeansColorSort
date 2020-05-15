using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning;
using KMeansColorSort.Models;

namespace KMeansColorSort.Services
{
    interface IColorSortService
    {
        IEnumerable<ColorModel> ClusterSort(IList<ColorModel> colors, int clusterCount, 
            double hueWeight, double saturationWeight, double lightnessWeight, int seed = 305419896);
        IEnumerable<ColorModel> HslSort(IList<ColorModel> colors);
    }

    class ColorSortService : IColorSortService
    {
        private const int _defaultSeed = 0x12345678;

        public IEnumerable<ColorModel> ClusterSort(IList<ColorModel> colors, int clusterCount,
            double hueWeight, double saturationWeight, double lightnessWeight, int seed = _defaultSeed)
        {
            Accord.Math.Random.Generator.Seed = seed;

            var colorInputs = colors.Select(x =>
                new double[] { (x.Hue / 360) * hueWeight, x.Saturation * saturationWeight, x.Lightness * lightnessWeight })
                .ToArray();

            var kmeans = new KMeans(clusterCount);
            var clusters = kmeans.Learn(colorInputs);
            var labels = clusters.Decide(colorInputs);

            var clusteredColors = new List<ColorModel>();

            for (int i = 0; i < labels.Length; i++)
            {
                var color = colors[i];
                var model = new ColorModel(color.R, color.G, color.B, color.A, color.Hue, color.Saturation, color.Lightness)
                {
                    Band = labels[i]
                };

                clusteredColors.Add(model);
            }

            return clusteredColors.OrderBy(x => x.Band)
                .ThenBy(x => x.Hue)
                .ThenBy(x => x.Saturation)
                .ThenBy(x => x.Lightness);
        }

        public IEnumerable<ColorModel> HslSort(IList<ColorModel> colors)
        {
            return colors.OrderBy(x => x.Hue)
                .ThenBy(x => x.Saturation)
                .ThenBy(x => x.Lightness);
        }
    }
}
