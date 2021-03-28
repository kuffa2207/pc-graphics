using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class Grad : MorphOps
    {
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            BitmapData srcData = sourceImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            int stride = srcData.Stride;
            byte[] resultBuffer1 = new byte[bytes];
            byte[] sourceBuffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, sourceBuffer, 0, sourceBuffer.Length);
            myPixel pix1;
            myPixel pix2;
            Filters er1;
            Filters er2;
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
                    er1 = new Erosion();
                    er2 = new Dilation();
                    pix1 = er1.calculateNewPixelColor(sourceBuffer, srcData.Stride, srcData.Height, x, y * srcData.Stride);
                    pix2 = er2.calculateNewPixelColor(sourceBuffer, srcData.Stride, srcData.Height, x, y * srcData.Stride);
                    resultBuffer1[x + y * stride] = (byte)Clamp(Clamp(pix2.B, 0, 255) - Clamp(pix1.B, 0, 255), 0, 255);
                    resultBuffer1[x + y * stride + 1] = (byte)Clamp(Clamp(pix2.G, 0, 255) - Clamp(pix1.G, 0, 255), 0, 255);
                    resultBuffer1[x + y * stride + 2] = (byte)Clamp(Clamp(pix2.R, 0, 255) - Clamp(pix1.R, 0, 255), 0, 255);
                    resultBuffer1[x + y * stride + 3] = 255;
                }
            }
            Bitmap resultImage = new Bitmap(width, height);
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer1, 0, resultData.Scan0, resultBuffer1.Length);
            sourceImage.UnlockBits(srcData);
            resultImage.UnlockBits(resultData);
            return resultImage;
        }
    };
}
