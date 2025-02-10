using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImSh = SixLabors.ImageSharp;
using Color = RayTracing.Vec3;
using System.Security.Cryptography;

namespace RayTracing;

public struct JpegColor
{
    public byte R;
    public byte G;
    public byte B;
}
public static class ColorUtils
{
    private static readonly Interval intensity = new Interval(0.000, 0.999);
    public static JpegColor[] WriteColor(Color[] unconvertedImage, int width, int height)
    {
        JpegColor[] convertedImage = new JpegColor[width * height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var R = unconvertedImage[i * width + j].x;
                var G = unconvertedImage[i * width + j].y;
                var B = unconvertedImage[i * width + j].z;

                convertedImage[i * width + j].R = (byte)(255.999 * intensity.Clamp(R));
                convertedImage[i * width + j].G = (byte)(255.999 * intensity.Clamp(G));
                convertedImage[i * width + j].B = (byte)(255.999 * intensity.Clamp(B));

            }
        }
        return convertedImage;
    }
    public static void SaveAsJpeg(JpegColor[] ppmImage, int width, int height)
    {
        ImSh.Image<ImSh::PixelFormats.Rgb24> image = new(width, height);
        image.DangerousTryGetSinglePixelMemory(out Memory<ImSh::PixelFormats.Rgb24> memory);
        var span = memory.Span;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                span[i * width + j].R = (byte)ppmImage[i * width + j].R;
                span[i * width + j].G = (byte)ppmImage[i * width + j].G;
                span[i * width + j].B = (byte)ppmImage[i * width + j].B;
            }
        }
        string imageName = $"../../../ImagesJpeg/SphereWithNoShadowAcne.jpeg";
        ImSh.Formats.Jpeg.JpegEncoder encoder = new();
        using FileStream fileStream = new FileStream(imageName, FileMode.OpenOrCreate, FileAccess.Write);
        encoder.Encode(image, fileStream);
        image.Dispose();
    }

}
