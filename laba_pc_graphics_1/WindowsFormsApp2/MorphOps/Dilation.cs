using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class Dilation : MorphOps
    {
        unsafe public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            myPixel pix;
            float maxR = 0f;
            float maxG = 0f;
            float maxB = 0f;
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
                        if ((sourceBuffer[idx + idy + 2] > maxR))
                        {
                            maxR = sourceBuffer[idx + idy + 2];
                        }
                        if ((sourceBuffer[idx + idy + 1] > maxG))
                        {
                            maxG = sourceBuffer[idx + idy + 1];
                        }
                        if ((sourceBuffer[idx + idy] > maxB))
                        {
                            maxB = sourceBuffer[idx + idy];
                        }
                    }
                }
            pix.R = ((int)maxR);
            pix.G = ((int)maxG);
            pix.B = ((int)maxB);
            pix.A = (255);
            return pix;
        }
    };
}
