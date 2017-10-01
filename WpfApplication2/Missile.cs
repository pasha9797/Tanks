using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace WpfApplication2
{
    public class Missile
    {
        public Vector3D speed;

        public Point3D pos;

        public void Move()
        {
            double t = ((double)C.UpdateInterval / 1000);
            pos.Z += speed.Z * t; //s=v*t;
            pos.X += (speed.X * t) * C.metersize;
            pos.Y -= (speed.Y * t) * C.metersize;
            //speed.Y -= C.downAcceleration * t*C.metersize;
        }
        public void ChangeDirection(bool left, bool right, bool up, bool down)
        {
            double angleY = (left && !right) ? -C.correctionDelta : (right && !left ? C.correctionDelta : 0);
            double angleX = (up && !down) ? -C.correctionDelta : (down && !up ? C.correctionDelta : 0);
            if (angleX != 0)
            {
                double Y = speed.Y;
                speed.Y = speed.Y * Math.Cos(ToRadian(angleX)) - speed.Z * Math.Sin(ToRadian(angleX));
                speed.Z = Y * Math.Sin(ToRadian(angleX)) + speed.Z * Math.Cos(ToRadian(angleX));
            }
            if (angleY != 0)
            {
                double X = speed.X;
                speed.X = speed.X * Math.Cos(ToRadian(angleY)) + speed.Z * Math.Sin(ToRadian(angleY));
                speed.Z = -X * Math.Sin(ToRadian(angleY)) + speed.Z * Math.Cos(ToRadian(angleY));
            }
        }
        private double ToRadian(double angle)
        {
            return angle * Math.PI / 180;
        }

        public Missile(double x, double y, double angle)
        {
            pos.Z = 0;
            pos.X = x;
            pos.Y = y;
            speed = new Vector3D(0, C.missileSpeed * Math.Sin(ToRadian(angle)), C.missileSpeed * Math.Cos(ToRadian(angle)));
        }
    }
}
