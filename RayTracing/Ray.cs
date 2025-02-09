using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RayTracing;
using Point3 = RayTracing.Vec3;
public struct Ray
{
    private Point3 _origin;
    private Point3 _direction;
    public Point3 Origin => _origin;
    public Vec3 Direction => _direction;
    public Ray() { _origin = new Point3(); _direction = new Vec3(); }
    public Ray(Point3 origin, Vec3 direction)
    {
        _origin = origin; _direction = direction;
    }
    public Point3 At(double t) => _origin + t * _direction;


}
