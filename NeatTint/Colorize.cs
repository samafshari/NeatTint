using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Math;

namespace NeatTint
{
    public class Colorize
    {
        public float Saturation;
        public float Lightness;
        public float Value;
        public float TintR, TintG, TintB;
        public string InputPath;

        public static float ToFloat(byte b) => (float)b / 255.0f;
        public static byte ToByte(float f) => (byte)Min(255.0f, Max(0, (f * 255.0f)));
        static float Lerp(float a, float b, float t) => b * t + a * (1 - t);
        static float Blend3(float left, float mid, float right, float pos)
        {
            if (pos < 0) return Lerp(left, mid, pos + 1);
            if (pos > 0) return Lerp(mid, right, pos);
            return mid;
        }

        byte Shade(byte bpixel, float tint) //works only for rgb, keep a as is
        {
            if (Lightness <= -1) return 0;
            if (Lightness > 1) return 1;

            var pixel = ToFloat(bpixel);
            var color = Lerp(0.5f, tint, Saturation);
            if (Lightness >= 0)
                return ToByte(Blend3(0.0f, color, 1.0f, 2.0f * (1.0f - Lightness) * (Value - 1) + 1));

            return ToByte(Blend3(0.0f, color, 1.0f, 2.0f * (1.0f + Lightness) * Value - 1));
        }

        public Bitmap Work()
        {
            if (!File.Exists(InputPath)) return null;
            // Create a new bitmap.
            Bitmap bmp = new Bitmap(InputPath);

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            volatile byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set every third value to 255. A 24bpp bitmap will look red.  
            //for (int counter = 2; counter < rgbValues.Length; counter += 3)
            //    rgbValues[counter] = 255;
            Parallel.For(0, rgbValues.Length, i =>
            {
                var el = i % 4;
                if (el == 3) return;
                if (el == 2) Shade(rgbValues[i], TintR);
                else if (el == 1) Shade(rgbValues[i], TintG);
                else Shade(rgbValues[i], TintB);
            });
            //for (int i = 0; i < rgbValues.Length; i++)
            //{
            //    rgbValues[i] = Shade(rgbValues[i++], TintB);
            //    rgbValues[i] = Shade(rgbValues[i++], TintG);
            //    rgbValues[i] = Shade(rgbValues[i++], TintR);
            //}

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        public BitmapImage GetBitmapImage()
        {
            var bmp = Work();
            if (bmp == null) return null;

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}
