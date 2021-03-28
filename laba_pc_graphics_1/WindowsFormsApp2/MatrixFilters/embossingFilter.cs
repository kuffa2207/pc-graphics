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
    class embossingFilter : MatrixFilter
    {
        public embossingFilter()
        {
            createEmbossinKernel();
        }

        protected void createEmbossinKernel()
        {
            kernel = new float[,] { { 0,  1,  0},
                                    { 1,  0, -1},
                                    { 0, -1,  0}};
        }
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            myPixel tmp = base.calculateNewPixelColor(sourceBuffer, stride, height, x, y);
            tmp.R += 255;
            tmp.G += 255;
            tmp.B += 255;
            tmp.R /= 2;
            tmp.G /= 2;
            tmp.B /= 2;
            tmp.A = (255);
            return tmp;
        }
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
            myPixel pix;
            Filters er;
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
                    er = new GrayScaleFilter();
                    pix = er.calculateNewPixelColor(sourceBuffer, srcData.Stride, srcData.Height, x, y * srcData.Stride);
                    resultBuffer[x + y * stride] = (byte)Clamp(pix.B, 0, 255);
                    resultBuffer[x + y * stride + 1] = (byte)Clamp(pix.G, 0, 255);
                    resultBuffer[x + y * stride + 2] = (byte)Clamp(pix.R, 0, 255);
                    resultBuffer[x + y * stride + 3] = (byte)Clamp(pix.A, 0, 255);
                }
            }
            for (int i = 0; i < bytes; ++i)
            {
                sourceBuffer[i] = resultBuffer[i];
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
                    pix = calculateNewPixelColor(sourceBuffer, srcData.Stride, srcData.Height, x, y * srcData.Stride);
                    resultBuffer[x + y * stride] = (byte)Clamp(pix.B, 0, 255);
                    resultBuffer[x + y * stride + 1] = (byte)Clamp(pix.G, 0, 255);
                    resultBuffer[x + y * stride + 2] = (byte)Clamp(pix.R, 0, 255);
                    resultBuffer[x + y * stride + 3] = (byte)Clamp(pix.A, 0, 255);
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
