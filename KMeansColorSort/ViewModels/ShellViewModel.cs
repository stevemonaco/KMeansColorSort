using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using Stylet;
using Accord.MachineLearning;
using KMeansColorSort.Models;
using KMeansColorSort.Services;

namespace KMeansColorSort.ViewModels
{
    class ShellViewModel : Screen
    {
        private readonly IColorGeneratorService _colorGeneratorService;
        private Random _random = new Random();

        private BindableCollection<ColorModel> _unsortedColors;
        public BindableCollection<ColorModel> UnsortedColors
        {
            get => _unsortedColors;
            set => SetAndNotify(ref _unsortedColors, value);
        }

        private BindableCollection<ColorModel> _hslSortedColors;
        public BindableCollection<ColorModel> HslSortedColors
        {
            get => _hslSortedColors;
            set => SetAndNotify(ref _hslSortedColors, value);
        }

        private BindableCollection<ColorModel> _clusterSortedColors;
        public BindableCollection<ColorModel> ClusterSortedColors
        {
            get => _clusterSortedColors;
            set => SetAndNotify(ref _clusterSortedColors, value);
        }

        private int _clusterCount = 5;
        public int ClusterCount
        {
            get => _clusterCount;
            set
            {
                SetAndNotify(ref _clusterCount, value);
                PerformClusterSort();
            }
        }

        private bool _showBands;
        public bool ShowBands
        {
            get => _showBands;
            set => SetAndNotify(ref _showBands, value);
        }

        private double _hueWeight = 10;
        public double HueWeight
        {
            get => _hueWeight;
            set
            {
                SetAndNotify(ref _hueWeight, value);
                PerformClusterSort();
            }
        }

        private double _saturationWeight = 5;
        public double SaturationWeight
        {
            get => _saturationWeight;
            set
            {
                SetAndNotify(ref _saturationWeight, value);
                PerformClusterSort();
            }
        }

        private double _lightnessWeight = 3;
        public double LightnessWeight
        {
            get => _lightnessWeight;
            set
            {
                SetAndNotify(ref _lightnessWeight, value);
                PerformClusterSort();
            }
        }

        public ShellViewModel(IColorGeneratorService colorGeneratorService)
        {
            _colorGeneratorService = colorGeneratorService;
        }

        protected override void OnViewLoaded()
        {
            ShowSystemDrawingColors();
            base.OnViewLoaded();
        }

        public void ShowSystemDrawingColors()
        {
            var colors = _colorGeneratorService.CreateSystemDrawingColors();
            UnsortedColors = new BindableCollection<ColorModel>(colors);

            var hslSorted = UnsortedColors.OrderBy(x => x.Hue)
                .ThenBy(x => x.Saturation)
                .ThenBy(x => x.Lightness);

            HslSortedColors = new BindableCollection<ColorModel>(hslSorted);
            PerformClusterSort();
        }

        public void ShowRandomColors()
        {
            var colors = _colorGeneratorService.CreateRandomColors(256, _random);
            UnsortedColors = new BindableCollection<ColorModel>(colors);

            var hslSorted = UnsortedColors.OrderBy(x => x.Hue)
                .ThenBy(x => x.Saturation)
                .ThenBy(x => x.Lightness);

            HslSortedColors = new BindableCollection<ColorModel>(hslSorted);
            PerformClusterSort();
        }

        private void PerformClusterSort()
        {
            Accord.Math.Random.Generator.Seed = 0x12345678;

            var colorInputs = UnsortedColors.Select(x => 
                new double[] { (x.Hue / 360) * HueWeight, x.Saturation * SaturationWeight, x.Lightness * LightnessWeight })
                .ToArray();
            
            var kmeans = new KMeans(ClusterCount);
            var clusters = kmeans.Learn(colorInputs);
            var labels = clusters.Decide(colorInputs);

            var cc = new ColorConverter();
            var colors = new List<ColorModel>();

            for (int i = 0; i < labels.Length; i++)
            {
                var color = UnsortedColors[i];
                var model = new ColorModel(color.R, color.G, color.B, color.A, color.Hue, color.Saturation, color.Lightness);

                model.Band = labels[i];
                colors.Add(model);
            }

            var sorted = colors.OrderBy(x => x.Band)
                .ThenBy(x => x.Hue)
                .ThenBy(x => x.Saturation)
                .ThenBy(x => x.Lightness);

            ClusterSortedColors = new BindableCollection<ColorModel>(sorted);
        }
    }
}
