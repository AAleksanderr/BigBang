using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using Boom.Model;

namespace Boom.ViewModel
{
    class ViewModel
    {
        private MainWindow currentWindow;
        private List<Pix> Pixs = new List<Pix>();
        private ObservableCollection<EllipsPosition> EllipsPositions = new ObservableCollection<EllipsPosition>();
        private const int NumberOfPixes = 10;
        private const int NumberOfIterations = 100;

        public ViewModel(MainWindow window)
        {
            currentWindow = window;
            CreatePixes();
        }

        public void Start()
        {
            ShowPixes();
            CalculateIteration();
        }

        public void CreatePixes()
        {
            var rnd = new Random();
            for (int i = 0; i < NumberOfPixes; i++)
            {
                var newPix = new Pix()
                {
                    X = rnd.Next(100, 300),
                    Y = rnd.Next(100, 300),
                    Z = rnd.Next(100, 300),
                    VX = (rnd.Next(1000)-500) / 1000f,
                    VY = (rnd.Next(1000)-500) / 1000f,
                    VZ = (rnd.Next(1000)-500) / 1000f
                };
                Pixs.Add(newPix);
                var newEllipsPosition = PixToEllipsPosition(newPix);
                EllipsPositions.Add(newEllipsPosition);
            }
        }

        public EllipsPosition PixToEllipsPosition(Pix pix)
        {
            var ellipsPos = new EllipsPosition
            {
                X = (int)(pix.X + pix.Z * 0.1),
                Y = (int)(pix.Y + pix.Z * 0.1),
                Size = (int)(1 / pix.Z * 10)+1
            };
            return ellipsPos;
        }
        public void CalculateIteration()
        {
            foreach (var pix in Pixs)
            foreach (var pix1 in Pixs)
            {
                if (pix1 == pix)
                    break;
                var tempX = pix1.X - pix.X;
                var tempY = pix1.Y - pix.Y;
                var tempZ = pix1.Z - pix.Z;
                var length = tempX * tempX + tempY * tempY + tempZ * tempZ;
                pix.VX += tempX / length;
                pix.X += pix.VX;
                pix.VY += tempY / length;
                pix.Y += pix.VY;
                pix.VZ += tempZ / length;
                pix.Z += pix.VZ;
            }
        }

        public void ShowPixes()
        {
            
            foreach (var elli in EllipsPositions)
            {
                var ellipse = new Ellipse()
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Fill = Brushes.Black,
                    Height = 5,
                    Width = 5
                };
                currentWindow.Grid.Children.Add(ellipse);
            }
        }
    }
}
