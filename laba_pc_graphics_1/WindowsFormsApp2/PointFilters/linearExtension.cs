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
    class linearExtension : Filters
    {
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            throw new NotImplementedException();
        }

        unsafe public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            BitmapData srcData = sourceImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            int stride = srcData.Stride;
            byte[] resultBuffer = new byte[bytes];
            double[] max = new double[3];
            for (int i = 0; i < 3; ++i)
            {
                max[i] = 0d;
            }
            double[] min = new double[3];
            for (int i = 0; i < 3; ++i)
            {
                min[i] = 255d;
            }
            byte* srcPtr = (byte*)srcData.Scan0;
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
                    for (int i = 0; i < 3; ++i)
                    {

                        if (max[i] < srcPtr[x + y * srcData.Stride + i])
                            max[i] = srcPtr[x + y * srcData.Stride + i];
                        if (min[i] > srcPtr[x + y * srcData.Stride + i])
                            min[i] = srcPtr[x + y * srcData.Stride + i];
                    }
                }
            }
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
                    for (int i = 0; i < 3; ++i)
                        resultBuffer[x + y * stride + i] = (byte)Clamp((int)((srcPtr[x + y * stride + i] - min[i]) * (255 / (max[i] - min[i]))), 0, 255);
                    resultBuffer[x + y * stride + 3] = 255;
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