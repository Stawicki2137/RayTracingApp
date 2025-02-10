using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;

public static class Rtfunc
{
    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
    public static double RandomDouble(double min = 0.0, double max = 1.0)
    {
        return min + (max - min) * Random.Shared.NextDouble();
    }

}
