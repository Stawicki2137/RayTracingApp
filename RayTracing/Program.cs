using Color = RayTracing.Vec3;

namespace RayTracing;

internal class Program
{
    static void Main(string[] args)
    {
        int imageWidth = 256;
        int imageHeight = 256;

        Color[] image = new Color[imageWidth * imageHeight];
        for (int j = 0; j < imageHeight; j++)
        {
            for (int i = 0; i < imageWidth; i++)
            {
                var pixelColor = new Color((double)i / (double)(imageWidth - 1), (double)j / (double)(imageHeight - 1), 0.2);
                image[j * imageWidth + i] = pixelColor;
            }
        }
        JpegColor[] imageJpeg = ColorUtils.WriteColor(image, imageWidth, imageHeight);
        ColorUtils.SaveAsJpeg(imageJpeg, imageWidth, imageHeight);

    }
}
