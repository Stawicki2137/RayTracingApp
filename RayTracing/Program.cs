using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;
using Color = RayTracing.Vec3;
using Point3 = RayTracing.Vec3;

namespace RayTracing;


internal class Program
{


    static void Main(string[] args)
    {

        HittableList world = new HittableList();

        var materialGround = new Lambertian(new Color(0.8, 0.8, 0.0));
        var materialCenter = new Lambertian(new Color(0.1, 0.2, 0.5));
        var materialLeft = new Dialectric(1.50);
        var materialBubble = new Dialectric(1.00 / 1.50);
        var materialRight = new Metal(new Color(0.8, 0.6, 0.2), 1.0);

        world.Add(new Sphere(new Point3(0.0, -100.5, -1.0), 100.0, materialGround));
        world.Add(new Sphere(new Point3(0.0, 0.0, -1.2), 0.5, materialCenter));
        world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), 0.5, materialLeft));
        world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), 0.4, materialBubble));
        world.Add(new Sphere(new Point3(1.0, 0.0, -1.0), 0.5, materialRight));

        ColorUtils.SetImageName = "DistantView";

        Camera camera = new Camera();
        camera.AspectRatio = 16.0 / 9.0;
        camera.ImageWidth = 1200;
        camera.SamplesPerPixel = 100;
        camera.MaxDepth = 50;

        camera.VFov = 90;
        camera.LookFrom = new Point3(-2, 2, 1);
        camera.LookAt = new Point3(0, 0, -1);
        camera.Vup = new Vec3(0, 1, 0);
        camera.Render(world);

    }
}
