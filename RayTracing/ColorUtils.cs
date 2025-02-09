using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = RayTracing.Vec3;

namespace RayTracing;

public static class ColorUtils
{
    public static void WriteColor(Color pixelColor)
    {
        var r = pixelColor.x;
        var g = pixelColor.y;
        var b = pixelColor.z;

        int rbyte = (int)(255.999 * r);
        int gbyte = (int)(255.999 * g);
        int bbyte = (int)(255.999 * b);

        Console.WriteLine($"{rbyte} {gbyte} {bbyte}");
    }
  
}
