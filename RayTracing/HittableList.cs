using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;

public class HittableList : Hittable
{
    public List<Hittable> Objects = new List<Hittable>();
    public HittableList() { }
    public HittableList(Hittable obj) { Add(obj); }
    public void Clear() { Objects.Clear(); }
    public void Add(Hittable obj) { Objects.Add(obj); }

    public override bool Hit(ref Ray ray, double rayTmin, double rayTmax, ref HitRecord hitRecord)
    {
        bool hitAnything = false;
        var closestSoFar = rayTmax;

        foreach (var obj in Objects)
        {
            if (obj.Hit(ref ray, rayTmin, closestSoFar, ref hitRecord))
            {
                hitAnything = true;
                closestSoFar = hitRecord.T;
            }
        }

        return hitAnything;

    }
}

