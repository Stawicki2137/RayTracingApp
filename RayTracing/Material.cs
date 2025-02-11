using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;
using Color = RayTracing.Vec3;

public interface IMaterial
{
    public bool Scatter(Ray rayIn, HitRecord record, ref Color attenuation, ref Ray scattered);

}
public class Material : IMaterial
{
    public virtual bool Scatter(Ray rayIn, HitRecord record, ref Color attenuation, ref Ray scattered)
    {
        return true;
    }
}
public class Metal : Material
{
    private Color _albedo;
    public Metal(Color albedo)
    {
        _albedo = albedo;
    }
    public override bool Scatter(Ray rayIn, HitRecord record, ref Color attenuation, ref Ray scattered)
    {
        Vec3 reflected = Vec3.Reflect(rayIn.Direction, record.Normal);
        scattered = new Ray(record.P, reflected);
        attenuation = _albedo;
        return true;
    }
}

public class Lambertian : Material
{
    private Color _albedo;
    public Lambertian(Color albedo)
    {
        _albedo = albedo;
    }
    public override bool Scatter(Ray rayIn, HitRecord record, ref Color attenuation, ref Ray scattered)
    {
        var scatterDirection = record.Normal + Vec3.RandomUnitVector();
        if (scatterDirection.NearZero())
            scatterDirection = record.Normal;
        scattered = new Ray(record.P, scatterDirection);
        attenuation = _albedo;
        return true;
    }
}
