using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class MaxBrightnessFilter : Filters
    {
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            myPixel pix;
            int k = 30;
            pix.R = Clamp(sourceBuffer[x + 2 + y] + k, 0, 255);
            pix.G = Clamp(sourceBuffer[x + 1 + y] + k, 0, 255);
            pix.B = Clamp(sourceBuffer[x + y] + k, 0, 255);
            pix.A = (255);
            return pix;
        }
    };
}
