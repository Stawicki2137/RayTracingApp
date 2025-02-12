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
    private AxisAlignedBoundingBox _boundingBox = new AxisAlignedBoundingBox();
    public HittableList() { }
    public HittableList(Hittable obj) { Add(obj); }
    public void Clear() { Objects.Clear(); }
    public void Add(Hittable obj) 
    {
        Objects.Add(obj);
        _boundingBox = new AxisAlignedBoundingBox(_boundingBox, obj.BoundingBox());
    }

    public override bool Hit(ref Ray ray, Interval rayT, ref HitRecord hitRecord)
    {
        bool hitAnything = false;
        var closestSoFar = rayT.Max;

        foreach (var obj in Objects)
        {
            if (obj.Hit(ref ray, new Interval(rayT.Min, closestSoFar), ref hitRecord))
            {
                hitAnything = true;
                closestSoFar = hitRecord.T;
            }
        }

        return hitAnything;

    }
    public override AxisAlignedBoundingBox BoundingBox() => _boundingBox;
}

