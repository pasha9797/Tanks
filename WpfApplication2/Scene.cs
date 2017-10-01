using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApplication2
{
    public class Scene
    {        
        public Point AimPosition;
        public Size wSize, fSize;
        public Tank tank;
        public Missile missile;
        public int[] Heights = new int[] { 810, 860, 970 };//координата Y в пикселях для каждой дальности
        public int[] Sizes = new int[] { 80, 110, 180 }; //Ширина танка для каждой дальности
        public int[] Farnesses = new int[] { 1800, 1200, 600 }; //Дальность в метрах
        Random rand = new Random();

        public void CheckVisibility()
        {
            if (tank != null)
            {
                if (tank.pos.X < 0 || tank.pos.X > fSize.Width)
                    Generate();
            }
        }
        public void CheckHit()
        {
            if (missile != null)
            {
                if (tank != null)
                {
                    if (missile.pos.Z >= Farnesses[(int)tank.farness])
                        Explode();
                }
                else
                    if (missile.pos.Z > Farnesses[0])
                    Explode();
            }
        }
        private void Explode()
        {
            if (tank != null && IsHit())
            {
                tank = null;
                Generate();
            }
            missile = null;
        }
        private bool IsHit()
        {
            if (missile.pos.X >= tank.pos.X && missile.pos.X <= tank.pos.X + Sizes[(int)tank.farness] &&
                    missile.pos.Y >= tank.pos.Y && missile.pos.Y <= tank.pos.Y + Sizes[(int)tank.farness] / 4)
                return true;
            else return false;
        }
        public void Generate()
        {
            Direction direction = (Direction)rand.Next(2);
            Farness farness = (Farness)rand.Next(3);
            double xStart;
            if (direction == Direction.RIGHT)
                xStart = Sizes[(int)farness] / 2;
            else
                xStart = fSize.Width - Sizes[(int)farness] / 2;

            tank = new Tank(xStart, Heights[(int)farness], direction, farness);
        }
        public void ProceedAimMove(double dX, double dY)
        {
            Point temp = new Point(AimPosition.X + dX * 4, AimPosition.Y + dY * 4);
            if (temp.X > 0 && temp.X < fSize.Width - wSize.Width &&
                temp.Y > 0 && temp.Y < fSize.Height - wSize.Height)
                AimPosition = temp;
        }
        public void LaunchMissile()
        {
            if (missile == null)
            {
                missile = new Missile(AimPosition.X + wSize.Width / 2 - 12, AimPosition.Y + wSize.Height / 2 - 25, C.startAngle);
            }
        }
        public Scene(double x, double y, Size windowSize, Size fieldSize)
        {
            AimPosition.X = x;
            AimPosition.Y = y;
            wSize = windowSize;
            fSize = fieldSize;
            Generate();
        }
    }
}
