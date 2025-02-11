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
public class Material: IMaterial
{
    public virtual bool Scatter(Ray rayIn, HitRecord record, ref Color attenuation, ref Ray scattered)
    {
        return true;
    }
}
