using System;
using System.Collections.Generic;
using Stylet;
using KMeansColorSort.Models;
using KMeansColorSort.Services;
using System.Threading.Tasks;
using System.Threading;

namespace KMeansColorSort.ViewModels
{
    class ShellViewModel : Screen
    {
        private readonly IColorGeneratorService _colorGeneratorService;
        private readonly IColorSortService _colorSortService;
        private readonly Random _random = new Random();
        private SemaphoreSlim _operationSemaphore = new SemaphoreSlim(1, 1);

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

        private int _colorCount;
        public int ColorCount
        {
            get => _colorCount;
            set => SetAndNotify(ref _colorCount, value);
        }

        private int _clusterCount = 5;
        public int ClusterCount
        {
            get => _clusterCount;
            set
            {
                SetAndNotify(ref _clusterCount, value);
                _ = SortClusterColors();
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
                _ = SortClusterColors();
            }
        }

        private double _saturationWeight = 5;
        public double SaturationWeight
        {
            get => _saturationWeight;
            set
            {
                SetAndNotify(ref _saturationWeight, value);
                _ = SortClusterColors();
            }
        }

        private double _lightnessWeight = 3;
        public double LightnessWeight
        {
            get => _lightnessWeight;
            set
            {
                SetAndNotify(ref _lightnessWeight, value);
                _ = SortClusterColors();
            }
        }

        public ShellViewModel(IColorGeneratorService colorGeneratorService, IColorSortService colorSortService)
        {
            _colorGeneratorService = colorGeneratorService;
            _colorSortService = colorSortService;
        }

        protected override async void OnInitialActivate()
        {
            await ShowSystemDrawingColors();
            base.OnInitialActivate();
        }

        protected override void OnClose()
        {
            _operationSemaphore?.Dispose();
            base.OnClose();
        }

        public async Task ShowSystemDrawingColors() => 
            await ShowColors(_colorGeneratorService.CreateSystemDrawingColors());

        public async Task ShowRandomColors() =>
            await ShowColors(_colorGeneratorService.CreateRandomColors(256, _random));

        private async Task ShowColors(IEnumerable<ColorModel> colors)
        {
            await _operationSemaphore.WaitAsync();

            try
            {
                await Task.Run(() =>
                {
                    UnsortedColors = new BindableCollection<ColorModel>(colors);
                    ColorCount = UnsortedColors.Count;

                    var hslSorted = _colorSortService.HslSort(UnsortedColors);
                    HslSortedColors = new BindableCollection<ColorModel>(hslSorted);

                    var clusterSorted = _colorSortService.ClusterSort(UnsortedColors, ClusterCount, HueWeight, SaturationWeight, LightnessWeight);
                    ClusterSortedColors = new BindableCollection<ColorModel>(clusterSorted);
                });
            }
            finally
            {
                _operationSemaphore.Release();
            }
        }

        private async Task SortClusterColors()
        {
            await _operationSemaphore.WaitAsync();

            try
            {
                await Task.Run(() =>
                {
                    var clusterSorted = _colorSortService.ClusterSort(UnsortedColors, ClusterCount, HueWeight, SaturationWeight, LightnessWeight);
                    ClusterSortedColors = new BindableCollection<ColorModel>(clusterSorted);
                });
            }
            finally
            {
                _operationSemaphore.Release();
            }
        }
    }
}
