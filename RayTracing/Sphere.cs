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
    public Sphere(Point3 center, double radius)
    {
        _center = center;
        _radius = Math.Max(radius, 0);
    }
    public override bool Hit(ref Ray ray, double rayTmin, double rayTmax, ref HitRecord hitRecord)
    {
        Vec3 originCenter = _center - ray.Origin;
        var a = ray.Direction.LengthSquared();
        var h = Vec3.Dot(ray.Direction, originCenter);
        var c = originCenter.LengthSquared() - _radius * _radius;

        var discriminant = h * h - a * c;
        if(discriminant<0) return false;

        var sqrtd = Math.Sqrt(discriminant);
        var root = (h - sqrtd) / a;
        if(root<=rayTmin||rayTmax<=root)
        {
            root = (h+sqrtd) / a;
            if(root<=rayTmin||rayTmax<=root ) 
                return false;
        }
        hitRecord.T = root;
        hitRecord.P = ray.At(hitRecord.T);
        hitRecord.Normal = (hitRecord.P - _center)/_radius;
        return true;
    }
}
