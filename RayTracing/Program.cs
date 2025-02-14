using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;
using System.Xml.Serialization;
using Color = RayTracing.Vec3;
using Point3 = RayTracing.Vec3;

namespace RayTracing;


internal class Program
{
    public static void BouncingSpheresOnCheckerGround()
    {
        HittableList world = new HittableList();

        var checkerGroundMaterial = new Checker(0.32, new Color(0.0, 0.0, 0.8), new Color(0.9, 0.9, 0.9));
        world.Add(new Sphere(new Point3(0, -1000, 0), 1000, new Lambertian(checkerGroundMaterial)));

        for (int a = -9; a < 9; a++)
        {
            for (int b = -9; b < 9; b++)
            {
                var chooseMat = Rtfunc.RandomDouble();
                Point3 center = new Point3(1.15 * a + 0.9 * Rtfunc.RandomDouble(), 0.2, 1.15 * b + 0.9 * Rtfunc.RandomDouble());

                if ((center - new Point3(4, 0.2, 0)).Length() > 0.9)
                {
                    Material sphereMaterial = new Material();

                    if (chooseMat < 0.76)
                    {
                        var albedo = Vec3.Random() * Vec3.Random();
                        sphereMaterial = new Lambertian(albedo);
                        var center2 = center + new Vec3(0, Rtfunc.RandomDouble(0, 0.35), 0);
                        world.Add(new Sphere(center, center2, 0.2, sphereMaterial));
                    }
                    else if (chooseMat < 0.91)
                    {
                        var albedo = Vec3.Random(0.5, 1);
                        var fuzz = Rtfunc.RandomDouble(0, 0.5);
                        sphereMaterial = new Metal(albedo, fuzz);
                        world.Add(new Sphere(center, 0.2, sphereMaterial));
                    }
                    else
                    {
                        sphereMaterial = new Dialectric(1.5);
                        world.Add(new Sphere(center, 0.2, sphereMaterial));
                    }
                }
            }
        }
        var dialectric1 = new Dialectric(1.5);
        world.Add(new Sphere(new Point3(0, 1, 0), 1.0, dialectric1));
        var lambertian1 = new Lambertian(new Color(0.4, 0.2, 0.1));
        world.Add(new Sphere(new Point3(-4, 1, 0), 1.0, lambertian1));
        var metal1 = new Metal(new Color(0.7, 0.6, 0.5), 0.0);
        world.Add(new Sphere(new Point3(4, 1, 0), 1.0, metal1));
        world = new HittableList(new BoundingVolumeHierarchyNode(world));



        ColorUtils.SetImageName = "BouncingSpheresOnTheCheckerGround6";

        Camera camera = new Camera();
        camera.AspectRatio = 16.0 / 9.0;
        camera.ImageWidth = 800;
        camera.SamplesPerPixel = 50;
        camera.MaxDepth = 30;

        camera.VFov = 20;
        camera.LookFrom = new Point3(13, 2, 3);
        camera.LookAt = new Point3(0, 0, 0);
        camera.Vup = new Vec3(0, 1, 0);
        camera.DefocusAngle = 0.6;
        camera.FocusDistance = 10.0;

        Stopwatch sw = Stopwatch.StartNew();
        camera.Render(world);
        sw.Stop();
        Console.WriteLine($"Rendering completed in {sw.ElapsedMilliseconds * 1e-3,2} s");

    }

    static void Main(string[] args)
    {
        BouncingSpheresOnCheckerGround();  
    }
}

