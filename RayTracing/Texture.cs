using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;
using Color = RayTracing.Vec3;
using Point3 = RayTracing.Vec3;
public class Texture
{
    public virtual Color Value(double u, double v, Point3 point)
    {
        return new Color();
    }
}
public class Checker : Texture
{
    private double _invScale;
    private Texture _even;
    private Texture _odd;
    public Checker(double scale, Texture even, Texture odd)
    {
        _invScale = 1.0 / scale;
        _even = even;
        _odd = odd;
    }
    public Checker(double scale, Color color1, Color color2)
        : this(scale, new SolidColor(color1), new SolidColor(color2)) { }
    public override Color Value(double u, double v, Point3 point)
    {
        var xInteger = (int)(Math.Floor(_invScale * point.x));
        var yInteger = (int)(Math.Floor(_invScale * point.y));
        var zInteger = (int)(Math.Floor(_invScale * point.z));

        bool isEven = (xInteger + yInteger + zInteger) % 2 == 0;
        return isEven ? _even.Value(u, v, point) : _odd.Value(u, v, point);
    }
}
public class SolidColor : Texture
{
    private Color _albedo;
    public SolidColor(Color albedo)
    {
        _albedo = albedo;
    }
    public SolidColor(double red, double green, double blue)
        : this(new Color(red, green, blue)) { }
    public override Color Value(double u, double v, Point3 point)
    {
        return _albedo;
    }
}