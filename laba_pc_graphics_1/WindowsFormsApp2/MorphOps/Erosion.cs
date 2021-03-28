using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class Erosion : MorphOps
    {
        unsafe public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            myPixel pix;
            float minR = 255f;
            float minG = 255f;
            float minB = 255f;
            y /= stride;
            int ht = kernel.GetLength(1);
            int wh = kernel.GetLength(0);
            for (int k = -(ht / 2); k <= (ht / 2); ++k)
                for (int l = -(wh / 2); l <= (wh / 2); ++l)
                {
                    int idx = Clamp(x + k * 4, 0, stride - 4);
                    int idy = Clamp(y + l, 0, height - 1) * stride;
                    if (kernel[(wh / 2) + l, (ht / 2) + k] != 0f)
                    {
                        if ((sourceBuffer[idx + idy + 2] < minR))
                        {
                            minR = sourceBuffer[idx + idy + 2];
                        }
                        if ((sourceBuffer[idx + idy + 1] < minG))
                        {
                            minG = sourceBuffer[idx + idy + 1];
                        }
                        if ((sourceBuffer[idx + idy] < minB))
                        {
                            minB = sourceBuffer[idx + idy];
                        }
                    }
                }
            pix.R = ((int)minR);
            pix.G = ((int)minG);
            pix.B = ((int)minB);
            pix.A = (255);
            return pix;
        }
    };
}
