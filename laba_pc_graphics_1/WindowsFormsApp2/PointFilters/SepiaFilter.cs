using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class SepiaFilter : Filters
    {
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            myPixel pix;
            double k = 20;
            int intensity = Intensity(sourceBuffer, x + y);
            pix.R = Clamp((int)(intensity + 2 * k), 0, 255);
            pix.G = Clamp((int)(intensity + 0.5 * k), 0, 255);
            pix.B = Clamp((int)(intensity - k), 0, 255);
            pix.A = (255);
            return pix;
        }
    };
}
