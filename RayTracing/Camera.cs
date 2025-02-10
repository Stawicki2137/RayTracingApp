using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing;
using Color = RayTracing.Vec3;
using Point3 = RayTracing.Vec3;
public class Camera
{
    public double aspectRatio = 1.0;
    public int imageWidth = 100;

    private int _imageHeight;
    private Point3 _cameraCenter;
    private Point3 _pixel00Location;
    private Vec3 _pixelDeltaU;
    private Vec3 _pixelDeltaV;
    public void Render(Hittable world)
    {
        Initialize();
        Color[] image = new Color[imageWidth * _imageHeight];
        for (int j = 0; j < _imageHeight; j++)
        {
            for (int i = 0; i < imageWidth; i++)
            {
                var pixelCenter = _pixel00Location + (i * _pixelDeltaU) + (j * _pixelDeltaV);
                var rayDirection = pixelCenter - _cameraCenter;
                Ray ray = new Ray(_cameraCenter, rayDirection);
                Color pixelColor = RayColor(ray, world);
                image[j * imageWidth + i] = pixelColor;
            }
        }
        JpegColor[] imageJpeg = ColorUtils.WriteColor(image, imageWidth, _imageHeight);
        ColorUtils.SaveAsJpeg(imageJpeg, imageWidth, _imageHeight);

    }
    private void Initialize()
    {
        _imageHeight = (int)(imageWidth / aspectRatio);
        _imageHeight = (_imageHeight < 1) ? 1 : _imageHeight;
        _cameraCenter = new Point3(0, 0, 0);
        var focalLength = 1.0;
        var viewportHeight = 2.0;
        var viewportWidth = viewportHeight * (double)(imageWidth) / (double)_imageHeight;
        var viewportU = new Vec3(viewportWidth, 0, 0); //Horizontal
        var viewportV = new Vec3(0, -viewportHeight, 0); //Vertical

        _pixelDeltaU = viewportU / imageWidth;
        _pixelDeltaV = viewportV / _imageHeight;
        var viewportUpperLeft = _cameraCenter - new Vec3(0, 0, focalLength) - viewportU / 2 - viewportV / 2;
        _pixel00Location = viewportUpperLeft + 0.5 * (_pixelDeltaU + _pixelDeltaV);


    }
    private Color RayColor(Ray ray, Hittable world)
    {
        HitRecord record = new HitRecord();
        if (world.Hit(ref ray, new Interval(0), ref record))
        {
            return 0.5 * (record.Normal + new Color(1, 1, 1));
        }
        Vec3 unitDirection = Vec3.UnitVector(ray.Direction);
        var a = 0.5 * (unitDirection.y + 1.0);
        return (1.0 - a) * new Color(1.0, 1.0, 1.0) + a * new Color(0.5, 0.7, 1.0);
    }

}
