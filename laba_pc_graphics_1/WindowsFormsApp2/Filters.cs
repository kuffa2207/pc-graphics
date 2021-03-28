using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApp2
{
    struct myPixel
    {
        public int R, G, B, A;

    };
    abstract class Filters
    {
        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
        public int Intensity(byte[] sourceBuffer, int x)
        {
            return (int)((0.299f * sourceBuffer[x + 2]) + (0.587f * sourceBuffer[x + 1]) + (0.114f * sourceBuffer[x]));
        }
        public abstract myPixel calculateNewPixelColor(byte[] sourceBuffer, int stride, int height, int x, int y); 
        public virtual Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker) 
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