using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class GrayScaleFilter : Filters
    {
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            myPixel pix;
            int intensity = Intensity(sourceBuffer, x + y);

            pix.B = pix.G = pix.R = Clamp(intensity, 0, 255);
            pix.A = (255);
            return pix;
        }
    };
}
