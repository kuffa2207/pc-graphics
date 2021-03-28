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
    abstract class BorderAllocation : MatrixFilter
    {
        protected abstract void changeFilterKernel(int ch);
        public override myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y)
        {
            float resRx;
            float resGx;
            float resBx;
            float resRy;
            float resGy;
            float resBy;
            changeFilterKernel(1);
            myPixel tmp = base.calculateNewPixelColor(sourceBuffer, stride, height, x, y);
            resRx = tmp.R;
            resGx = tmp.G;
            resBx = tmp.B;
            changeFilterKernel(2);
            tmp = base.calculateNewPixelColor(sourceBuffer, stride, height, x, y);
            resRy = tmp.R;
            resGy = tmp.G;
            resBy = tmp.B;
            double tmpR = Math.Sqrt(resRx * resRx + resRy * resRy);
            double tmpG = Math.Sqrt(resGx * resGx + resGy * resGy);
            double tmpB = Math.Sqrt(resBx * resBx + resBy * resBy);
            tmp.R = Clamp((int)tmpR, 0, 255);
            tmp.G = Clamp((int)tmpG, 0, 255);
            tmp.B = Clamp((int)tmpB, 0, 255);
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
                    er = new MedianFilter();
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
