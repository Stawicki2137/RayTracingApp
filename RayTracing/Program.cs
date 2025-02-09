using Color = RayTracing.Vec3;

namespace RayTracing;

internal class Program
{
    static void Main(string[] args)
    {
        int imageWidth = 256;
        int imageHeight = 256;

        Console.Write($"P3\n{imageWidth} {imageHeight}\n255\n");
        for(int j = 0; j<imageHeight; j++)
        {
            for(int i = 0; i<imageHeight; i++)
            {
                var pixelColor = new Color((double)i/(double)(imageWidth-1), (double)j/(double)(imageHeight-1),0.2);
                ColorUtils.WriteColor(pixelColor);
            }
        }
    }
}
