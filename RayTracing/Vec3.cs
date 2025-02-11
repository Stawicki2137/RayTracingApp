using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;


public struct Vec3
{
    public double[] e = new double[3];
    public Vec3() { }
    public Vec3(double e0, double e1, double e2) { e[0] = e0; e[1] = e1; e[2] = e2; }
    public double x => e[0];
    public double y => e[1];
    public double z => e[2];

    public static Vec3 operator -(Vec3 v)
    {
        return new Vec3(-v.e[0], -v.e[1], -v.e[2]);
    }

    public double this[int i]
    {
        set { e[i] = value; }
        get { return e[i]; }
    }

    public static Vec3 operator +(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.e[0] + v2.e[0], v1.e[1] + v2.e[1], v1.e[2] + v2.e[2]);
    }

    public static Vec3 operator +(Vec3 v, double a)
    {
        return new Vec3(v.e[0] + a, v.e[1] + a, v.e[2] + a);
    }

    public static Vec3 operator -(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.e[0] - v2.e[0], v1.e[1] - v2.e[1], v1.e[2] - v2.e[2]);
    }

    public static Vec3 operator -(Vec3 v, double a)
    {
        return new Vec3(v.e[0] - a, v.e[1] - a, v.e[2] - a);
    }

    public static Vec3 operator *(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.e[0] * v2.e[0], v1.e[1] * v2.e[1], v1.e[2] * v2.e[2]);
    }

    public static Vec3 operator *(Vec3 v, double a)
    {
        return new Vec3(v.e[0] * a, v.e[1] * a, v.e[2] * a);
    }

    public static Vec3 operator *(double a, Vec3 v)
    {
        return new Vec3(v.e[0] * a, v.e[1] * a, v.e[2] * a);
    }

    public static Vec3 operator /(Vec3 v, double a)
    {
        return new Vec3(v.e[0] / a, v.e[1] / a, v.e[2] / a);
    }
    public static double Dot(Vec3 v1, Vec3 v2)
    {
        return v1.e[0] * v2.e[0] + v1.e[1] * v2.e[1] + v1.e[2] * v2.e[2];
    }

    public static Vec3 Cross(Vec3 v1, Vec3 v2)
    {
        return new Vec3(
        v1.e[1] * v2.e[2] - v1.e[2] * v2.e[1],
        v1.e[2] * v2.e[0] - v1.e[0] * v2.e[2],
        v1.e[0] * v2.e[1] - v1.e[1] * v2.e[0]);
    }

    public double LengthSquared()
    {
        return e[0] * e[0] + e[1] * e[1] + e[2] * e[2];
    }
    public bool NearZero()
    {
        var s = 1e-8;
        return (Math.Abs(e[0]) < s) && (Math.Abs(e[1]) < s) && (Math.Abs(e[2]) < s);
    }
    public double Length()
    {
        return Math.Sqrt(LengthSquared());
    }
    public static Vec3 UnitVector(Vec3 v)
    {
        return v / v.Length();
    }
    public static Vec3 Random()
    {
        return new Vec3(Rtfunc.RandomDouble(), Rtfunc.RandomDouble(), Rtfunc.RandomDouble());
    }
    public static Vec3 Random(double min, double max)
    {
        return new Vec3(Rtfunc.RandomDouble(min, max), Rtfunc.RandomDouble(min, max), Rtfunc.RandomDouble(min, max));
    }
    public static Vec3 RandomUnitVector()
    {
        while (true)
        {
            var p = Vec3.Random(-1, 1);
            var lensq = p.LengthSquared();
            if (1e-160 < lensq && lensq <= 1)
                return p / Math.Sqrt(lensq);
        }
    }
    public static Vec3 RandomOnHemisphere(Vec3 normal)
    {
        Vec3 onUnitSphere = Vec3.RandomUnitVector();
        if (Vec3.Dot(onUnitSphere, normal) > 0.0)
            return onUnitSphere;
        else
            return -onUnitSphere;
    }
    public static Vec3 Reflect(Vec3 v, Vec3 n)
    {
        return v - 2 * Vec3.Dot(v, n) * n;
    }
    public static Vec3 Refract(Vec3 rayDirUnitVec, Vec3 normal, double etaiOverEtat)
    {
        double cosTheta = Math.Min(Vec3.Dot(-rayDirUnitVec, normal), 1.0);
        Vec3 rayOutPerpendicular = etaiOverEtat * (rayDirUnitVec + cosTheta * normal);
        Vec3 rayOutParallel = -Math.Sqrt(Math.Abs(1.0 - rayOutPerpendicular.LengthSquared())) * normal;
        return rayOutPerpendicular + rayOutParallel;
    }
    public static Vec3 RandomPointInsideUnitDisk()
    {
        while (true)
        {
            var p = new Vec3(Rtfunc.RandomDouble(-1, 1), Rtfunc.RandomDouble(-1, 1), 0);
            if (p.LengthSquared() < 1)
                return p;
        }
    }

    public override string ToString()
    {
        return $"{e[0]} {e[1]} {e[2]}";
    }
};

