using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class MatrixFilter : Filters
    {
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            myPixel pix;
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            float resultR = 0;
            float resultG = 0;
            float resultB = 0;
            y /= stride;
            for (int l = -radiusX; l <= radiusX; ++l)
                for (int k = -radiusY; k <= radiusY; ++k)
                {
                    int idx = Clamp(x + k * 4, 0, stride - 4);
                    int idy = Clamp(y + l, 0, height - 1) * stride;
                    resultR += sourceBuffer[idx + idy + 2] * kernel[k + radiusX, l + radiusY];
                    resultG += sourceBuffer[idx + idy + 1] * kernel[k + radiusX, l + radiusY];
                    resultB += sourceBuffer[idx + idy] * kernel[k + radiusX, l + radiusY];
                }
            pix.R = ((int)resultR);
            pix.G = ((int)resultG);
            pix.B = ((int)resultB);
            pix.A = (255);
            return pix;
        }
        protected float[,] kernel = null;
        protected MatrixFilter() { }
        public MatrixFilter(float[,] kernel)
        {
            this.kernel = kernel;
        }

    };
}
