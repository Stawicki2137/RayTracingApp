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
public class Dialectric : Material
{
    private double _refractionIndex; // air - material 

    public Dialectric(double refractionIndex)
    {
        _refractionIndex = refractionIndex;
    }
    private double Reflectance(double cosine, double refractionIndex)
    {
        var fresnelCoefficient = (1.0 - refractionIndex) / (1.0 + refractionIndex);
        fresnelCoefficient *= fresnelCoefficient;
        return fresnelCoefficient + (1.0 - fresnelCoefficient) * Math.Pow((double)(1.0 - cosine), 5.0);
    }
    public override bool Scatter(Ray rayIn, HitRecord record, ref Color attenuation, ref Ray scattered)
    {
        attenuation = new Color(1.0, 1.0, 1.0);
        double refractionIndex = record.FrontFace ? ((double)1.0 / _refractionIndex) : _refractionIndex;

        Vec3 unitDirection = Vec3.UnitVector(rayIn.Direction);
        double cosTheta = Math.Min(Vec3.Dot(-unitDirection, record.Normal), 1.0);
        double sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);
        bool cannotRefract = refractionIndex * sinTheta > 1.0;
        Vec3 direction = new Vec3();

        if (cannotRefract)
            direction = Vec3.Reflect(unitDirection, record.Normal);
        else
            direction = Vec3.Refract(unitDirection, record.Normal, refractionIndex);
        scattered = new Ray(record.P, direction, rayIn.Time);
        return true;
    }

}

public class Metal : Material
{
    private Color _albedo;
    private double _fuzziness;
    public Metal(Color albedo, double fuzziness)
    {
        _albedo = albedo;
        _fuzziness = fuzziness < 1 ? fuzziness : 1;
    }
    public override bool Scatter(Ray rayIn, HitRecord record, ref Color attenuation, ref Ray scattered)
    {
        Vec3 reflected = Vec3.Reflect(rayIn.Direction, record.Normal);
        reflected = Vec3.UnitVector(reflected) + (_fuzziness * Vec3.RandomUnitVector());
        scattered = new Ray(record.P, reflected, rayIn.Time);
        attenuation = _albedo;
        return (Vec3.Dot(scattered.Direction, record.Normal) > 0);
    }
}

public class Lambertian : Material
{
    private Texture _texture;
    public Lambertian(Color albedo)
    {
        _texture = new SolidColor(albedo);
    }
    public Lambertian(Texture texture)
    {
        _texture = texture;
    }

    public override bool Scatter(Ray rayIn, HitRecord record, ref Color attenuation, ref Ray scattered)
    {
        var scatterDirection = record.Normal + Vec3.RandomUnitVector();
        if (scatterDirection.NearZero())
            scatterDirection = record.Normal;
        scattered = new Ray(record.P, scatterDirection, rayIn.Time);
        attenuation = _texture.Value(record.U, record.V, record.P);
        return true;
    }
}
