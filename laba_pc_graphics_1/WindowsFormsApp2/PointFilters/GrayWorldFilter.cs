using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//работай пж
namespace WindowsFormsApp2
{
    class GrayWorldFilter : Filters
    {
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            myPixel pix;
            pix.B = (sourceBuffer[x + y]);
            pix.G = (sourceBuffer[x + 1 + y]);
            pix.R = (sourceBuffer[x + 2 + y]);
            pix.A = (255);
            return pix;
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            BitmapData srcData = sourceImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            byte[] resultBuffer = new byte[bytes];
            byte[] sourceBuffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, sourceBuffer, 0, sourceBuffer.Length);
            myPixel pix;
            double pR = 0, pG = 0, pB = 0;
            for (int y = 0; y < height; ++y)
            {
                worker.ReportProgress((int)((float)y / height * 100));
                if (worker.CancellationPending)
                {
                    sourceImage.UnlockBits(srcData);
                    return null;
                }
                for (int x = 0; x < srcData.Stride; x += 4)
                {
                    pix = calculateNewPixelColor(sourceBuffer, srcData.Stride, srcData.Height, x, y * srcData.Stride);
                    pB += (byte)pix.B;
                    pG += (byte)pix.G;
                    pR += (byte)pix.R;
                }
            }
            double leng = bytes / 4;
            pR /= leng;
            pG /= leng;
            pB /= leng;
            double average = (pG + pB + pR) / 3;
            pR = average / pR;
            pG = average / pG;
            pB = average / pB;
            for (int y = 0; y < height; ++y)
            {
                worker.ReportProgress((int)((float)y / height * 100));
                if (worker.CancellationPending)
                {
                    sourceImage.UnlockBits(srcData);
                    return null;
                }
                for (int x = 0; x < srcData.Stride; x += 4)
                {
                    pix = calculateNewPixelColor(sourceBuffer, srcData.Stride, srcData.Height, x, y * srcData.Stride);
                    resultBuffer[x + y * srcData.Stride] = (byte)Clamp((int)(pix.B * pB), 0, 255);
                    resultBuffer[x + y * srcData.Stride + 1] = (byte)Clamp((int)(pix.G * pG), 0, 255);
                    resultBuffer[x + y * srcData.Stride + 2] = (byte)Clamp((int)(pix.R * pR), 0, 255);
                    resultBuffer[x + y * srcData.Stride + 3] = 255;
                }
            }
            Bitmap resultImage = new Bitmap(width, height);
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            sourceImage.UnlockBits(srcData);
            resultImage.UnlockBits(resultData);
            return resultImage;
        }
    };
}
