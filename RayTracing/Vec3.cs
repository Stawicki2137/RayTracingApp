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
    public Vec3(double e0, double e1, double e2)  { e[0] = e0; e[1] = e1; e[2] = e2; }
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
    public override string ToString()
    {
        return $"{e[0]} {e[1]} {e[2]}";
    }
};

