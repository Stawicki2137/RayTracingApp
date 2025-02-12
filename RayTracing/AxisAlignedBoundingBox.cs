using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;
using Point3 = RayTracing.Vec3;
public class AxisAlignedBoundingBox
{
    public Interval X, Y, Z;
    public AxisAlignedBoundingBox() { }
    public AxisAlignedBoundingBox(Interval x, Interval y, Interval z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public AxisAlignedBoundingBox(Point3 a, Point3 b)
    {
        X = (a[0] <= b[0]) ? new Interval(a[0], b[0]) : new Interval(b[0], a[0]);
        Y = (a[1] <= b[1]) ? new Interval(a[1], b[1]) : new Interval(b[1], a[1]);
        Z = (a[2] <= b[2]) ? new Interval(a[2], b[2]) : new Interval(b[2], a[2]);
    }
    public AxisAlignedBoundingBox(AxisAlignedBoundingBox a, AxisAlignedBoundingBox b)
    {
        X = new Interval(a.X, b.X);
        Y = new Interval(a.Y, b.Y);
        Z = new Interval(a.Z, b.Z);
    }
    public Interval AxisInterval(int n)
    {
        if (n == 1) return Y;
        if (n == 2) return Z;
        return X;
    }
    public int LongestAxis()
    {
        if (X.Size() > Y.Size())
            return X.Size() > Z.Size() ? 0 : 2;
        else
            return Y.Size() > Z.Size() ? 1 : 2;
    }
    public bool Hit(Ray ray, Interval rayT)
    {
        Point3 rayOrigin = ray.Origin;
        Vec3 rayDirection = ray.Direction;

        for (int axis = 0; axis < 3; axis++)
        {
            Interval ax = AxisInterval(axis);
            double axisDirectionInverse = 1.0 / rayDirection[axis];

            var t0 = (ax.Min - rayOrigin[axis]) * axisDirectionInverse;
            var t1 = (ax.Max - rayOrigin[axis]) * axisDirectionInverse;

            if (t0 < t1)
            {
                if (t0 > rayT.Min) rayT.Min = t0;
                if (t1 < rayT.Max) rayT.Max = t1;
            }
            else
            {
                if (t1 > rayT.Min) rayT.Min = t1;
                if (t0 < rayT.Max) rayT.Max = t0;
            }
            if (rayT.Max <= rayT.Min)
                return false;
        }
        return true;
    }
    public static readonly AxisAlignedBoundingBox Empty = new AxisAlignedBoundingBox(Interval.Empty, Interval.Empty, Interval.Empty);
    public static readonly AxisAlignedBoundingBox Universe = new AxisAlignedBoundingBox(Interval.Universe, Interval.Universe, Interval.Universe);
}

