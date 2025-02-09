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
                var r = (double)i/ (double)(imageWidth-1);
                var g = (double)j / (double)(imageHeight - 1);
                var b = 0.5;

                int ir = (int)(255.999*r);
                int ig = (int)(255.999*g);
                int ib = (int)(255.999 * b);

                Console.WriteLine($"{ir} {ig} {ib}");
            }
        }
    }
}
