using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;
using Point3 = RayTracing.Vec3;
public class HitRecord
{
    public Point3 P;
    public Vec3 Normal = new Vec3();
    public Material Material = new Material();
    public double T;
    public bool FrontFace;

    public void SetFaceNormal(Ray ray, Vec3 outwardNormal)
    {
        FrontFace = Vec3.Dot(ray.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }

}
public abstract class Hittable
{
    public abstract bool Hit(ref Ray ray, Interval rayT, ref HitRecord hitRecord);
}
