using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApplication2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Scene myScene;
        DispatcherTimer updater;
        Point oldCoords;
        Image curImg;
        public MainWindow()
        {
            InitializeComponent();
            myScene = new Scene(backgroundImage.Width / 2 - this.Width / 2, backgroundImage.Height / 2, new Size(this.Width, this.Height), new Size(backgroundImage.Width, backgroundImage.Height));
            updater = new DispatcherTimer();
            updater.Interval = new TimeSpan(0, 0, 0, 0, C.UpdateInterval);
            updater.Tick += new EventHandler(UpdateAllTheScene);
            updater.Start();
            UpdateAim();
        }
        public void UpdateAllTheScene(object sender, EventArgs e)
        {
            if (myScene.tank != null)
            {
                myScene.tank.Move();
                UpdateTank();
                myScene.CheckVisibility();
            }
            else
            {
                tankImage.Visibility = Visibility.Hidden;
                tankImage2.Visibility = Visibility.Hidden;
            }

            if (myScene.missile != null)
            {
                myScene.missile.Move();
                myScene.missile.ChangeDirection(Keyboard.IsKeyDown(Key.Left), Keyboard.IsKeyDown(Key.Right), Keyboard.IsKeyDown(Key.Up), Keyboard.IsKeyDown(Key.Down));
                UpdateMissile();
                myScene.CheckHit();
            }
            else
            {
                MissileIndicator.Visibility = Visibility.Hidden;
                Range.Visibility = Visibility.Hidden;
            }
        }
        public void UpdateTank()
        {
            if (myScene.tank != null)
            {
                switch (myScene.tank.dir)
                {
                    case Direction.RIGHT:
                        tankImage.Visibility = Visibility.Hidden;
                        tankImage2.Visibility = Visibility.Visible;
                        curImg = tankImage2;
                        break;
                    case Direction.LEFT:
                        tankImage.Visibility = Visibility.Visible;
                        tankImage2.Visibility = Visibility.Hidden;
                        curImg = tankImage;
                        break;
                }
                curImg.Width = myScene.Sizes[(int)myScene.tank.farness];

                double x, y;
                x = -myScene.AimPosition.X + myScene.tank.pos.X;
                y = -myScene.AimPosition.Y + myScene.tank.pos.Y;
                tank.Margin = new Thickness(x, y, -x, -y);

                double ix, iy;
                ix = x >= 0 ? (x <= this.Width - 17 ? x : this.Width - Indicator.Width - 17) : 0;
                iy = y > 0 ? (y <= this.Height - 38 ? y : this.Height - Indicator.Height - 38) : 0;
                Indicator.Margin = new Thickness(ix, iy, -ix, -iy);
            }
        }
        public void UpdateMissile()
        {
            if (myScene.missile != null)
            {
                MissileIndicator.Visibility = Visibility.Visible;
                Range.Visibility = Visibility.Visible;
                double x, y;
                x = -myScene.AimPosition.X + myScene.missile.pos.X;
                y = -myScene.AimPosition.Y + myScene.missile.pos.Y;
                MissileIndicator.Margin = new Thickness(x, y, -x, -y);
                Range.Margin = new Thickness(x+10, y, -x, -y);
                MissileIndicator.Width = 10 * (1 - myScene.missile.pos.Z / (myScene.Farnesses[0] *1.2));
                MissileIndicator.Height = MissileIndicator.Width;
                int metersleft= (int)Math.Round(myScene.Farnesses[(int)myScene.tank.farness] -myScene.missile.pos.Z);
                Range.Text = (metersleft>=0?metersleft:0).ToString() + " m";
            }

        }
        public void UpdateAim()
        {
            background.Margin = new Thickness(-myScene.AimPosition.X, -myScene.AimPosition.Y, myScene.AimPosition.X, myScene.AimPosition.Y);
        }
        private void mainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            oldCoords = e.GetPosition(this);
        }

        private void mainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                myScene.ProceedAimMove(e.GetPosition(this).X - oldCoords.X, e.GetPosition(this).Y - oldCoords.Y);
                UpdateAim();
                UpdateTank();
                UpdateMissile();
                oldCoords = e.GetPosition(this);

            }
        }

        private void mainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                myScene.LaunchMissile();
            }
        }
    }
}
