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
    class GlassFilter : Filters
    {
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            BitmapData srcData = sourceImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            int stride = srcData.Stride;
            byte[] resultBuffer = new byte[bytes];
            byte[] sourceBuffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, sourceBuffer, 0, sourceBuffer.Length);
            Random rand = new Random();
            myPixel pix;
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
                    pix = calculateNewPixelColor(sourceBuffer, srcData.Stride, srcData.Height, x, y, ref rand);
                    resultBuffer[x + y * stride + 2] = (byte)pix.R;
                    resultBuffer[x + y * stride + 1] = (byte)pix.G;
                    resultBuffer[x + y * stride] = (byte)pix.B;
                    resultBuffer[x + y * stride + 3] = (byte)pix.A;
                }
            }
            Bitmap resultImage = new Bitmap(width, height);
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            sourceImage.UnlockBits(srcData);
            resultImage.UnlockBits(resultData);
            return resultImage;
        }
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            throw new NotImplementedException();
        }
        public myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y, ref Random rand)
        {
            myPixel pix;
            int rvalx = (int)((rand.Next(0, 2) - 0.5) * 10);
            rvalx = Clamp(x + rvalx * 4, 0, stride - 4);
            int rvaly = (int)((rand.Next(0, 2) - 0.5) * 10);
            rvaly = Clamp(y + rvaly, 0, height - 2);
            pix.R = (byte)Clamp((sourceBuffer[rvalx + rvaly * stride + 2]), 0, 255);
            pix.G = (byte)Clamp((sourceBuffer[rvalx + rvaly * stride + 1]), 0, 255);
            pix.B = (byte)Clamp((sourceBuffer[rvalx + rvaly * stride]), 0, 255);
            pix.A = 255;
            return pix;
        }
    };
}
