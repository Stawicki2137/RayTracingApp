using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;
using Color = RayTracing.Vec3;
using Point3 = RayTracing.Vec3;
public class Camera
{
    public double AspectRatio = 1.0;
    public int ImageWidth = 100;
    public int SamplesPerPixel = 10;
    public int MaxDepth = 10; // Maximum number of ray bounces into scene
    public double VFov = 90; // Vertical view angle
    public Point3 LookFrom = new Point3(0, 0, 0);
    public Point3 LookAt = new Point3(0, 0, -1);
    public Vec3 Vup = new Vec3(0, 1, 0); // Camera-relative UP direction
    public double DefocusAngle = 0;
    public double FocusDistance = 10;

    private int _imageHeight;
    private Point3 _cameraCenter;
    private Point3 _pixel00Location;
    private Vec3 _pixelDeltaU;
    private Vec3 _pixelDeltaV;
    private Vec3 _u, _v, _w; // Camera frame basis vectors
    private Vec3 _defocusDiskU, _defocusDiskV;
    private double _pixelSamplesScale;
    public void Render(Hittable world)
    {
        Initialize();
        Color[] image = new Color[ImageWidth * _imageHeight];
        Parallel.For(0, _imageHeight, j =>
        {
            for (int i = 0; i < ImageWidth; i++)
            {
                Color pixelColor = new Color(0, 0, 0);
                for (int sample = 0; sample < SamplesPerPixel; sample++)
                {
                    Ray ray = GetRay(i, j);
                    pixelColor += RayColor(ray, MaxDepth, world);
                }
                image[j * ImageWidth + i] = pixelColor * _pixelSamplesScale;
            }
        });
        JpegColor[] imageJpeg = ColorUtils.WriteColor(image, ImageWidth, _imageHeight);
        ColorUtils.SaveAsJpeg(imageJpeg, ImageWidth, _imageHeight);

    }
    private void Initialize()
    {
        _imageHeight = (int)(ImageWidth / AspectRatio);
        _imageHeight = (_imageHeight < 1) ? 1 : _imageHeight;
        _pixelSamplesScale = (double)1.0 / (double)SamplesPerPixel;
        _cameraCenter = LookFrom;
        var theta = Rtfunc.DegreesToRadians(VFov);
        var h = Math.Tan(theta / 2);
        var viewportHeight = 2.0 * h * FocusDistance;
        var viewportWidth = viewportHeight * (double)(ImageWidth) / (double)_imageHeight;

        _w = Vec3.UnitVector(LookFrom - LookAt);
        _u = Vec3.UnitVector(Vec3.Cross(Vup, _w));
        _v = Vec3.Cross(_w, _u);

        var viewportU = viewportWidth * _u; //Horizontal
        var viewportV = viewportHeight * (-_v); //Vertical

        _pixelDeltaU = viewportU / ImageWidth;
        _pixelDeltaV = viewportV / _imageHeight;
        var viewportUpperLeft = _cameraCenter - (FocusDistance * _w) - viewportU / 2 - viewportV / 2;
        _pixel00Location = viewportUpperLeft + 0.5 * (_pixelDeltaU + _pixelDeltaV);

        var defocusRadius = FocusDistance * Math.Tan(Rtfunc.DegreesToRadians(DefocusAngle / 2));
        _defocusDiskU = _u * defocusRadius;
        _defocusDiskV = _v * defocusRadius;
    }
    private Ray GetRay(int i, int j)
    {
        var offset = SampleSquare();
        var pixelSample = _pixel00Location
           + ((i + offset.x) * _pixelDeltaU)
           + ((j + offset.y) * _pixelDeltaV);

        var rayOrigin = (DefocusAngle <= 0) ? _cameraCenter : DefocusDiskSample();
        var rayDirection = pixelSample - rayOrigin;
        return new Ray(rayOrigin, rayDirection);
    }
    private Point3 DefocusDiskSample()
    {
        var p = Vec3.RandomPointInsideUnitDisk();
        return _cameraCenter + (p[0] * _defocusDiskU) + (p[1] * _defocusDiskV);
    }
    private Vec3 SampleSquare()
    {
        return new Vec3(Rtfunc.RandomDouble() - 0.5, Rtfunc.RandomDouble() - 0.5, 0);
    }
    private Color RayColor(Ray ray, int depth, Hittable world)
    {
        if (depth <= 0)
            return new Color(0, 0, 0);

        HitRecord record = new HitRecord();
        if (world.Hit(ref ray, new Interval(0.001), ref record))
        {
            var scattered = new Ray();
            var attenuation = new Color();
            if (record.Material.Scatter(ray, record, ref attenuation, ref scattered))
                return attenuation * RayColor(scattered, depth - 1, world);
            return new Color(0, 0, 0);

        }
        Vec3 unitDirection = Vec3.UnitVector(ray.Direction);
        var a = 0.5 * (unitDirection.y + 1.0);
        return (1.0 - a) * new Color(1.0, 1.0, 1.0) + a * new Color(0.5, 0.7, 1.0);
    }

}
