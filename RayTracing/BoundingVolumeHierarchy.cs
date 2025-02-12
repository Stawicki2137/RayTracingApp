using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;

public class BoundingVolumeHierarchyNode : Hittable
{
    private Hittable _left;
    private Hittable _right;
    private AxisAlignedBoundingBox _boundingBox;
    public BoundingVolumeHierarchyNode(HittableList list)
        : this(list.Objects, 0, list.Objects.Count()) { }

    public BoundingVolumeHierarchyNode(List<Hittable> obj, int start, int end)
    {
        _boundingBox = AxisAlignedBoundingBox.Empty;
        for (int objIndex = start; objIndex < end; objIndex++)
        {
            _boundingBox = new AxisAlignedBoundingBox(_boundingBox, obj[objIndex].BoundingBox());
        }
        int axis = _boundingBox.LongestAxis();
        Comparison<Hittable> comparator;
        if (axis == 0)
            comparator = BoxXCompare;
        else if (axis == 1)
            comparator = BoxYCompare;
        else
            comparator = BoxZCompare;
        int objectSpan = end - start;
        if (objectSpan == 1)
        {
            _left = _right = obj[start];
        }
        else if (objectSpan == 2)
        {
            _left = obj[start];
            _right = obj[start + 1];
        }
        else
        {
            obj.Sort(start, objectSpan, Comparer<Hittable>.Create(comparator));

            var mid = start + objectSpan / 2;
            _left = new BoundingVolumeHierarchyNode(obj, start, mid);
            _right = new BoundingVolumeHierarchyNode(obj, mid, end);
        }
    }
    private static int BoxCompare(Hittable a, Hittable b, int axisIndex)
    {
        var aAxisInterval = a.BoundingBox().AxisInterval(axisIndex);
        var bAxisInterval = b.BoundingBox().AxisInterval(axisIndex);
        return aAxisInterval.Min.CompareTo(bAxisInterval.Min);
    }
    private static int BoxXCompare(Hittable a, Hittable b) => BoxCompare(a, b, 0);
    private static int BoxYCompare(Hittable a, Hittable b) => BoxCompare(a, b, 1);
    private static int BoxZCompare(Hittable a, Hittable b) => BoxCompare(a, b, 2);


    public override AxisAlignedBoundingBox BoundingBox() => _boundingBox;

    public override bool Hit(ref Ray ray, Interval rayT, ref HitRecord hitRecord)
    {
        if (!_boundingBox.Hit(ray, rayT))
            return false;
        bool hitLeft = _left.Hit(ref ray, rayT, ref hitRecord);
        bool hitRight = _right.Hit(ref ray, new Interval(rayT.Min, hitLeft ? hitRecord.T : rayT.Max), ref hitRecord);
        return hitLeft || hitRight;
    }
}
