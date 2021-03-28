using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class InvertFilter : Filters
    {
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            myPixel pix;
            pix.B = (255 - sourceBuffer[x + y]);
            pix.G = (255 - sourceBuffer[x + 1 + y]);
            pix.R = (255 - sourceBuffer[x + 2 + y]);
            pix.A = (255);
            return pix;
        }

    };
}
