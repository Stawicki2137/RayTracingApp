using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;
using Color = RayTracing.Vec3;
using Point3 = RayTracing.Vec3;

namespace RayTracing;


internal class Program
{
   
    public static Color RayColor(Ray ray,Hittable world)
    {
        HitRecord record = new HitRecord();
        if(world.Hit(ref ray,0,double.MaxValue,ref record))
        {
            return 0.5 * (record.Normal + new Color(1, 1, 1));
        }
        Vec3 unitDirection = Vec3.UnitVector(ray.Direction);
        var a = 0.5 * (unitDirection.y + 1.0);
        return (1.0 - a) * new Color(1.0, 1.0, 1.0) + a * new Color(0.5, 0.7, 1.0);
    }
    static void Main(string[] args)
    {
        var aspectRatio = (double)16.0 / (double)9.0;
        int imageWidth = 400;

        int imageHeight = (int)((double)imageWidth / aspectRatio);
        imageHeight = (imageHeight < 1) ? 1 : imageHeight;

        //World 
        HittableList world = new HittableList();
        world.Add(new Sphere(new Point3(0, 0, -1), 0.5));
        world.Add(new Sphere(new Point3(0, -100.5, -1), 100));

        //Camera
        var focalLength = 1.0;
        var viewportHeight = 2.0;
        var viewportWidth = viewportHeight * (double)(imageWidth) / (double)imageHeight;
        var cameraCenter = new Point3(0, 0, 0);

        var viewportU = new Vec3(viewportWidth, 0, 0); //Horizontal
        var viewportV = new Vec3(0, -viewportHeight, 0); //Vertical

        var pixelDeltaU = viewportU / imageWidth;
        var pixelDeltaV = viewportV / imageHeight;

        var viewportUpperLeft = cameraCenter - new Vec3(0, 0, focalLength) - viewportU / 2 - viewportV / 2;
        var pixel00_location = viewportUpperLeft + 0.5 * (pixelDeltaU + pixelDeltaV);

        Color[] image = new Color[imageWidth * imageHeight];
        for (int j = 0; j < imageHeight; j++)
        {
            for (int i = 0; i < imageWidth; i++)
            {
                var pixelCenter = pixel00_location + (i * pixelDeltaU) + (j * pixelDeltaV);
                var rayDirection = pixelCenter - cameraCenter;
                Ray ray = new Ray(cameraCenter, rayDirection);
                Color pixelColor = RayColor(ray,world);
                image[j * imageWidth + i] = pixelColor;
            }
        }
        JpegColor[] imageJpeg = ColorUtils.WriteColor(image, imageWidth, imageHeight);
        ColorUtils.SaveAsJpeg(imageJpeg, imageWidth, imageHeight);
    }
}
