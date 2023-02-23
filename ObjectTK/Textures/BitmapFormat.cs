using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace ObjectTK.Textures
{
    internal class BitmapFormat
    {
        public SizedInternalFormat InternalFormat;
        public PixelFormat PixelFormat;
        public PixelType PixelType;

#pragma warning disable CA1416
        private static readonly Dictionary<System.Drawing.Imaging.PixelFormat, BitmapFormat> FormatMap = new Dictionary<System.Drawing.Imaging.PixelFormat, BitmapFormat>()
        {
            {
                System.Drawing.Imaging.PixelFormat.Format24bppRgb, new BitmapFormat
                {
                    InternalFormat = SizedInternalFormat.Rgba8,
                    PixelFormat = PixelFormat.Bgr,
                    PixelType = PixelType.UnsignedByte
                }
            },
            {
                System.Drawing.Imaging.PixelFormat.Format32bppArgb, new BitmapFormat
                {
                    InternalFormat = SizedInternalFormat.Rgba8,
                    PixelFormat = PixelFormat.Bgra,
                    PixelType = PixelType.UnsignedByte
                }
            }
        };
#pragma warning restore CA1416

        protected BitmapFormat() { }

        static BitmapFormat()
        {
#pragma warning disable CA1416
            FormatMap.Add(System.Drawing.Imaging.PixelFormat.Format32bppRgb,
                FormatMap[System.Drawing.Imaging.PixelFormat.Format32bppArgb]);
            FormatMap.Add(System.Drawing.Imaging.PixelFormat.Canonical,
                FormatMap[System.Drawing.Imaging.PixelFormat.Format32bppArgb]);
#pragma warning restore CA1416
        }

        public static BitmapFormat Get(Bitmap bitmap)
        {
#pragma warning disable CA1416
            if (FormatMap.ContainsKey(bitmap.PixelFormat))
                return FormatMap[bitmap.PixelFormat];
#pragma warning restore CA1416

            throw new ArgumentException("Error: Unsupported Pixel Format " + bitmap.PixelFormat);
        }
    }
}