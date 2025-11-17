using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SpeedRush
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private double speed = 5;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += GameLoop;
            timer.Start();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            double top = Canvas.GetTop(Obstacle);
            Canvas.SetTop(Obstacle, top + speed);

            if (top > 450)
            {
                Canvas.SetTop(Obstacle, -30);
            }

            if (IsCollision())
            {
                timer.Stop();
                MessageBox.Show("Game Over!");
            }
        }

        private bool IsCollision()
        {
            double px = Canvas.GetLeft(Player);
            double py = Canvas.GetTop(Player);
            double ox = Canvas.GetLeft(Obstacle);
            double oy = Canvas.GetTop(Obstacle);

            return Math.Abs(px - ox) < 30 && Math.Abs(py - oy) < 30;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            double left = Canvas.GetLeft(Player);

            if (e.Key == Key.Left && left > 0)
                Canvas.SetLeft(Player, left - 20);
            if (e.Key == Key.Right && left < 300)
                Canvas.SetLeft(Player, left + 20);
        }
    }
}