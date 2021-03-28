using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class MedianFilter : MatrixFilter
    {
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            myPixel pix;
            int radiusX = 7 / 2;
            int radiusY = 7 / 2;
            y /= stride;
            List<double> listR = new List<double>();
            List<double> listG = new List<double>();
            List<double> listB = new List<double>();
            for (int l = -radiusX; l <= radiusX; ++l)
                for (int k = -radiusY; k <= radiusY; ++k)
                {
                    int idx = Clamp(x + k * 4, 0, stride - 4);
                    int idy = Clamp(y + l, 0, height - 1) * stride;
                    listR.Add(sourceBuffer[idx + idy + 2]);
                    listG.Add(sourceBuffer[idx + idy + 1]);
                    listB.Add(sourceBuffer[idx + idy]);
                }
            listR.Sort();
            listG.Sort();
            listB.Sort();
            pix.R = ((int)listR[listR.Count() / 2]);
            pix.G = ((int)listG[listG.Count() / 2]);
            pix.B = ((int)listB[listB.Count() / 2]);
            pix.A = (255);
            return pix;
        }
    };
}
