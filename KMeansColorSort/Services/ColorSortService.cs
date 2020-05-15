using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Accord.MachineLearning;
using KMeansColorSort.Models;

namespace KMeansColorSort.Services
{
    interface IColorSortService
    {
        IEnumerable<ColorModel> ClusterSort(IList<ColorModel> colors, int clusterCount, 
            double hueWeight, double saturationWeight, double lightnessWeight, int seed = 0x12345678);
        IEnumerable<ColorModel> HslSort(IList<ColorModel> colors);
    }

    class ColorSortService : IColorSortService
    {
        public IEnumerable<ColorModel> ClusterSort(IList<ColorModel> colors, int clusterCount,
            double hueWeight, double saturationWeight, double lightnessWeight, int seed)
        {
            var colorInputs = colors.Select(x =>
                new double[] { (x.Hue / 360) * hueWeight, x.Saturation * saturationWeight, x.Lightness * lightnessWeight })
                .ToArray();

            Accord.Math.Random.Generator.Seed = seed;
            var kmeans = new KMeans(clusterCount);
            var clusters = kmeans.Learn(colorInputs);
            var labels = clusters.Decide(colorInputs);

            return colors.Zip(labels, (color, label) =>
                new ColorModel(color.R, color.G, color.B, color.A, color.Hue, color.Saturation, color.Lightness, label))
                .OrderBy(x => x.Band)
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
