using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApplication2
{
    public enum Direction
    {
        LEFT,
        RIGHT
    }
    public enum Farness
    {
        FAR,
        MIDDLE,
        CLOSE
    }
    public class Tank
    {
        public Point pos;
        public Farness farness;

        public Direction dir;

        public void Move()
        {
            double t = ((double)C.UpdateInterval / 1000);
            switch (dir)
            {
                case Direction.LEFT:
                    pos.X -= (C.tankSpeed * t) * C.metersize;
                    break;
                case Direction.RIGHT:
                    pos.X += (C.tankSpeed * t) * C.metersize;
                    break;
            }
        }

        public Tank(double x, double y, Direction dir, Farness farness)
        {
            pos.X = x;
            pos.Y = y;
            this.dir = dir;
            this.farness = farness;
        }
    }
}
