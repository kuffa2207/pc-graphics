using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class WavesFilter : Filters
    {
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            myPixel pix;
            int nx = Clamp((x + (int)(20 * Math.Sin(2 * Math.PI * (y / stride) / 60)) * 4), 0, stride - 4);
            pix.R = Clamp(sourceBuffer[nx + 2 + y], 0, 255);
            pix.G = Clamp(sourceBuffer[nx + 1 + y], 0, 255);
            pix.B = Clamp(sourceBuffer[nx + y], 0, 255);
            pix.A = (255);
            return pix;
        }
    };
}
