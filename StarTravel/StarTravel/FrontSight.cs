using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Math;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarTravel
{
    class FrontSight : IDraw
    {
        public int DrawingPriority { get; private set; }

        public int Closely { get; private set; }

        public Size Size { get; private set; }

        public int SizeOfCross { get; private set; }

        public Rectangle RestrictedZone { get; private set; }

        private Point focusPoint;

        public Point FocusPoint
        {
            get => focusPoint;
            private set
            {
                focusPoint = value;
                ChangeFocusPoint?.Invoke(focusPoint);
            }
        }

        private int moveSpeed;

        public event Action<Point> ChangeFocusPoint;

        public FrontSight(int drawingPriority = 2, int closely = 0, Size? size = null, int sizeOfCross = 20, 
            Rectangle ? restrictedZone = null, Point? focusPoint = null, int moveSpeed = 20)
        {
            DrawingPriority = drawingPriority;
            Closely = closely;
            Size = size?? new Size(100, 100);
            SizeOfCross = sizeOfCross; 
            RestrictedZone = restrictedZone?? new Rectangle(new Point(0, 0), new Size(Game.Width, Game.Ship.HeightOfCab));
            FocusPoint = focusPoint?? Game.ScreenCenterPoint;
            this.moveSpeed = moveSpeed;
        }

        public void Draw()
        {
            Pen pen = new Pen(Color.BlueViolet, 2);
            Game.Buffer.Graphics.DrawLine(pen, FocusPoint.X - SizeOfCross / 2, FocusPoint.Y, FocusPoint.X + SizeOfCross / 2, FocusPoint.Y);
            Game.Buffer.Graphics.DrawLine(pen, FocusPoint.X, FocusPoint.Y - SizeOfCross / 2, FocusPoint.X, FocusPoint.Y + SizeOfCross / 2);
            Game.Buffer.Graphics.DrawEllipse(pen, FocusPoint.X - Size.Width / 2, FocusPoint.Y - Size.Height / 2,
                Size.Width, Size.Height);
        }

        public void Form_KeyDown(object sender, KeyEventArgs e)
        {
            int x = FocusPoint.X;
            int y = FocusPoint.Y;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    x = FocusPoint.X - moveSpeed;
                    break;
                case Keys.Up:
                    y = FocusPoint.Y - moveSpeed;
                    break;
                case Keys.Right:
                    x = FocusPoint.X + moveSpeed;
                    break;
                case Keys.Down:
                    y = FocusPoint.Y + moveSpeed;
                    break;
                default:
                    break;
            }
            if      (RestrictedZone.Contains(x, y))            { FocusPoint = new Point(x, y); }
            else if (RestrictedZone.Contains(FocusPoint.X, y)) { FocusPoint = new Point(FocusPoint.X, y); }
            else if (RestrictedZone.Contains(x, FocusPoint.Y)) { FocusPoint = new Point(x, FocusPoint.Y); }

        }

        public void Form_MouseMove(object sender, EventArgs e)
        {
            if (RestrictedZone.Contains((e as MouseEventArgs).X, (e as MouseEventArgs).Y))
            {
                FocusPoint = new Point((e as MouseEventArgs).X, (e as MouseEventArgs).Y);
            }
        }
    }
}
