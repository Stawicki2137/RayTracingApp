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

        var R = Math.Cos(Math.PI / 4);

        var materialLeft = new Lambertian(new Color(0, 0, 1));
        var materialRight = new Lambertian(new Color(1, 0, 0));

        world.Add(new Sphere(new Point3(-R, 0, -1), R, materialLeft));
        world.Add(new Sphere(new Point3(R, 0, -1), R, materialRight));

        ColorUtils.SetImageName = "Wide-angleView";

        Camera camera = new Camera();
        camera.AspectRatio = 16.0 / 9.0;
        camera.ImageWidth = 1200;
        camera.SamplesPerPixel = 100;
        camera.MaxDepth = 50;
        camera.VFov = 90;
        camera.Render(world);

    }
}
