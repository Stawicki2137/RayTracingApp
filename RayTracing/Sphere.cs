using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;
using Point3 = RayTracing.Vec3;
public class Sphere : Hittable
{
    private Ray _center;
    private double _radius;
    private Material _material;
    // Stationary sphere
    public Sphere(Point3 staticCenter, double radius, Material material)
    {
        _center = new Ray(staticCenter,new Vec3(0,0,0));
        _radius = Math.Max(radius, 0);
        _material = material;
    }
    // Moving sphere
    public Sphere(Point3 center1, Point3 center2, double radius, Material material)
    {
        _center = new Ray(center1, center2 - center1);
        _radius = Math.Max(radius, 0);
        _material = material;
    }
    public override bool Hit(ref Ray ray, Interval rayT, ref HitRecord hitRecord)
    {
        Point3 currentCenter = _center.At(ray.Time);
        Vec3 originCenter = currentCenter - ray.Origin;
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
        Vec3 outwardNormal = (hitRecord.P - currentCenter) / _radius;
        hitRecord.SetFaceNormal(ray, outwardNormal);
        hitRecord.Material = _material;
        return true;
    }
}
