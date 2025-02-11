using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;
using Point3 = RayTracing.Vec3;
public class Sphere : Hittable
{
    private Point3 _center;
    private double _radius;
    private Material _material;
    public Sphere(Point3 center, double radius, Material material)
    {
        _center = center;
        _radius = Math.Max(radius, 0);
        _material = material;
    }
    public override bool Hit(ref Ray ray, Interval rayT, ref HitRecord hitRecord)
    {
        Vec3 originCenter = _center - ray.Origin;
        var a = ray.Direction.LengthSquared();
        var h = Vec3.Dot(ray.Direction, originCenter);
        var c = originCenter.LengthSquared() - _radius * _radius;

        var discriminant = h * h - a * c;
        if (discriminant < 0) return false;

        var sqrtd = Math.Sqrt(discriminant);
        var root = (h - sqrtd) / a;
        if (!rayT.Surrounds(root))
        {
            root = (h + sqrtd) / a;
            if (!rayT.Surrounds(root))
                return false;
        }
        hitRecord.T = root;
        hitRecord.P = ray.At(hitRecord.T);
        Vec3 outwardNormal = (hitRecord.P - _center) / _radius;
        hitRecord.SetFaceNormal(ray, outwardNormal);
        hitRecord.Material = _material;
        return true;
    }
}
