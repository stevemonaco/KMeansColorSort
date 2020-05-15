namespace KMeansColorSort.Models
{
    class ColorModel
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }

        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Lightness { get; set; }

        public int Band { get; set; }

        public ColorModel()
        {
        }

        public ColorModel(int r, int g, int b, int a, double hue, double saturation, double lightness, int band = 0)
        {
            R = r;
            G = g;
            B = b;
            A = a;
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
            Band = band;
        }
    }
}
