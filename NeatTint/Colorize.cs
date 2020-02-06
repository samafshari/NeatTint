using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using static System.Math;

namespace NeatTint
{
    public class Colorize
    {
        public float Saturation { get; set; } = 1.0f;
        public float Lightness { get; set; } = 0.0f;
        public float Value { get; set; } = 0.5f;
        public float TintR { get; set; } = 1.0f;
        public float TintG { get; set; } = 1.0f;
        public float TintB { get; set; } = 1.0f;
        public float Strength { get; set; } = 0.7f;

        public string InputPath { get; set; }
        public string OutputPath { get; set; }

        Bitmap ogBmp;

        public static float ToFloat(byte b) => (float)b / 255.0f;
        public static byte ToByte(float f) => (byte)Min(255.0f, Max(0, (f * 255.0f)));
        static float Lerp(float a, float b, float t) => b * t + a * (1 - t);
        static float Blend3(float left, float mid, float right, float pos)
        {
            if (pos < 0) return Lerp(left, mid, pos + 1);
            if (pos > 0) return Lerp(mid, right, pos);
            return mid;
        }

        public void Load()
        {
            if (!File.Exists(InputPath)) return;
            ogBmp = new Bitmap(InputPath);
        }

        byte Shade(byte bpixel, float tint) //works only for rgb, keep a as is
        {
            if (Strength == 0) return bpixel;

            float _Shade()
            {
                if (Lightness <= -1) return 0;
                if (Lightness > 1) return 1;

                var color = Lerp(0.5f, tint, Saturation);
                if (Lightness >= 0)
                    return Blend3(0.0f, color, 1.0f, 2.0f * (1.0f - Lightness) * (Value - 1) + 1);

                return Blend3(0.0f, color, 1.0f, 2.0f * (1.0f + Lightness) * Value - 1);
            }

            var pixel = ToFloat(bpixel);
            var shaded = _Shade();
            return ToByte(Lerp(pixel, shaded, Strength));
            //if (Lightness <= -1) return 0;
            //if (Lightness > 1) return 1;

            //var pixel = ToFloat(bpixel);
            //var color = Lerp(0.5f, tint, Saturation);
            //if (Lightness >= 0)
            //    return ToByte(Blend3(0.0f, color, 1.0f, 2.0f * (1.0f - Lightness) * (Value - 1) + 1));

            //return ToByte(Blend3(0.0f, color, 1.0f, 2.0f * (1.0f + Lightness) * Value - 1));
        }

        public Bitmap Work()
        {
            if (ogBmp == null) return null;
            Bitmap bmp = new Bitmap(ogBmp);
            // Create a new bitmap.
            unsafe
            {   
                // Lock the bitmap's bits.  
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData bmpData =
                    bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    bmp.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
                int heightInPixels = bmpData.Height;
                int widthInBytes = bmpData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bmpData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bmpData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        int oldBlue = currentLine[x];
                        int oldGreen = currentLine[x + 1];
                        int oldRed = currentLine[x + 2];

                        currentLine[x] = Shade((byte)oldBlue, TintB);
                        currentLine[x + 1] = Shade((byte)oldGreen, TintG);
                        currentLine[x + 2] = Shade((byte)oldRed, TintR);
                    }
                });

                // Unlock the bits.
                bmp.UnlockBits(bmpData);

                return bmp;
            }
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

        public void Save()
        {
            if (!File.Exists(InputPath))
            {
                MessageBox.Show(caption: "Error", messageBoxText: "No input given.", button: MessageBoxButton.OK);
                return;
            }
            
            if (string.IsNullOrWhiteSpace(OutputPath))
            {
                MessageBox.Show(caption: "Error", messageBoxText: "No output given.", button: MessageBoxButton.OK);
                return;
            }

            if (File.Exists(OutputPath))
            {
                var ask = MessageBox.Show(caption: "Warning", messageBoxText: $"Output path already exists. Do you want to overwrite {OutputPath}?", button: MessageBoxButton.YesNo);
                if (ask == MessageBoxResult.No) return;
            }
            try
            {
                var bmp = Work();
                if (bmp == null) return;
                
                if (File.Exists(OutputPath)) File.Delete(OutputPath);
                using (var fs = new FileStream(OutputPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(caption: "Error", messageBoxText: ex.ToString(), button: MessageBoxButton.OK);
            }
        }
    }
}
