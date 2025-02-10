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
        world.Add(new Sphere(new Point3(0, 0, -1), 0.5));
        world.Add(new Sphere(new Point3(0, -100.5, -1), 100));
        Camera camera = new Camera();

        camera.AspectRatio = 16.0 / 9.0;
        camera.ImageWidth = 1200;
        camera.SamplesPerPixel = 50;
        camera.MaxDepth = 30;
        camera.Render(world);

    }
}
