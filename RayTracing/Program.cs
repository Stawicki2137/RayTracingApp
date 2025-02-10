using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;
using Color = RayTracing.Vec3;
using Point3 = RayTracing.Vec3;

namespace RayTracing;


internal class Program
{
    public static bool HitSphere(Point3 center, double radius, Ray ray)
    {
        Vec3 originCenter = center - ray.Origin;
        var a = Vec3.Dot(ray.Direction,ray.Direction);
        var b = -2.0 * Vec3.Dot(ray.Direction,originCenter);
        var c = Vec3.Dot(originCenter,originCenter) - radius*radius;
        var discriminant = b * b - 4 * a * c;
        return discriminant >= 0;
    }
    public static Color RayColor(Ray ray)
    {
        if (HitSphere(new Point3(0, 0, -1), 0.7, ray))
            return new Color(1.0, 0, 0.5);
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
                Color pixelColor = RayColor(ray);
                image[j * imageWidth + i] = pixelColor;
            }
        }
        JpegColor[] imageJpeg = ColorUtils.WriteColor(image, imageWidth, imageHeight);
        ColorUtils.SaveAsJpeg(imageJpeg, imageWidth, imageHeight);

    }
}
