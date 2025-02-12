using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImSh = SixLabors.ImageSharp;
using Color = RayTracing.Vec3;
using System.Security.Cryptography;
using SixLabors.ImageSharp.PixelFormats;

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

                R = LinearToGamma(R);
                G = LinearToGamma(G);
                B = LinearToGamma(B);

                convertedImage[i * width + j].R = (byte)(255.999 * intensity.Clamp(R));
                convertedImage[i * width + j].G = (byte)(255.999 * intensity.Clamp(G));
                convertedImage[i * width + j].B = (byte)(255.999 * intensity.Clamp(B));

            }
        }
        return convertedImage;
    }
    private static double LinearToGamma(double linearComponent)
    {
        if (linearComponent > 0)
            return Math.Sqrt(linearComponent);

        return 0;
    }
    public static string SetImageName = "Image1";
    public static void SaveAsJpeg(JpegColor[] ppmImage, int width, int height)
    {
        ImSh.Image<ImSh::PixelFormats.Rgb24> image = new(width, height);
        //image.DangerousTryGetSinglePixelMemory(out Memory<ImSh::PixelFormats.Rgb24> memory);
        //var span = memory.Span;
        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < height; y++)
            {
                Span<Rgb24> pixelRowSpan = accessor.GetRowSpan(y);
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    pixelRowSpan[x] = new Rgb24(
                        ppmImage[index].R,
                        ppmImage[index].G,
                        ppmImage[index].B);
                }
            }
        });
        string imageName = $"../../../ImagesJpeg/" + SetImageName + ".jpeg";
        ImSh.Formats.Jpeg.JpegEncoder encoder = new();
        using FileStream fileStream = new FileStream(imageName, FileMode.OpenOrCreate, FileAccess.Write);
        encoder.Encode(image, fileStream);
        image.Dispose();
    }

}
