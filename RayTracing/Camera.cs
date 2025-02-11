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

    private int _imageHeight;
    private Point3 _cameraCenter;
    private Point3 _pixel00Location;
    private Vec3 _pixelDeltaU;
    private Vec3 _pixelDeltaV;
    private double _pixelSamplesScale;
    public void Render(Hittable world)
    {
        Initialize();
        Color[] image = new Color[ImageWidth * _imageHeight];
        for (int j = 0; j < _imageHeight; j++)
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
        }
        JpegColor[] imageJpeg = ColorUtils.WriteColor(image, ImageWidth, _imageHeight);
        ColorUtils.SaveAsJpeg(imageJpeg, ImageWidth, _imageHeight);

    }
    private void Initialize()
    {
        _imageHeight = (int)(ImageWidth / AspectRatio);
        _imageHeight = (_imageHeight < 1) ? 1 : _imageHeight;
        _pixelSamplesScale = (double)1.0 / (double)SamplesPerPixel;
        _cameraCenter = new Point3(0, 0, 0);
        var focalLength = 1.0;
        var viewportHeight = 2.0;
        var viewportWidth = viewportHeight * (double)(ImageWidth) / (double)_imageHeight;
        var viewportU = new Vec3(viewportWidth, 0, 0); //Horizontal
        var viewportV = new Vec3(0, -viewportHeight, 0); //Vertical

        _pixelDeltaU = viewportU / ImageWidth;
        _pixelDeltaV = viewportV / _imageHeight;
        var viewportUpperLeft = _cameraCenter - new Vec3(0, 0, focalLength) - viewportU / 2 - viewportV / 2;
        _pixel00Location = viewportUpperLeft + 0.5 * (_pixelDeltaU + _pixelDeltaV);
    }
    private Ray GetRay(int i, int j)
    {
        var offset = SampleSquare();
        var pixelSample = _pixel00Location
           + ((i + offset.x) * _pixelDeltaU)
           + ((j + offset.y) * _pixelDeltaV);

        var rayOrigin = _cameraCenter;
        var rayDirection = pixelSample - rayOrigin;
        return new Ray(rayOrigin, rayDirection);
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
            if(record.Material.Scatter(ray,record,ref attenuation, ref scattered))
                return attenuation * RayColor(scattered,depth-1, world);
            return new Color(0,0,0);
           
        }
        Vec3 unitDirection = Vec3.UnitVector(ray.Direction);
        var a = 0.5 * (unitDirection.y + 1.0);
        return (1.0 - a) * new Color(1.0, 1.0, 1.0) + a * new Color(0.5, 0.7, 1.0);
    }

}
