using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class BaseObject
    {
        protected Point pos;
        protected Point dir;
        protected Size size;
        protected Size maxSize;
        protected int closely; //0 - очень близко, 10 - очень далеко
        protected int delay;
        protected string number;
        public BaseObject(Point pos, Point dir, Size size, int closely, int delay = 0, Size? maxSize = null, string number = "")
        {
            this.pos = pos;
            this.dir = dir;
            this.size = size;
            this.closely = closely <= 10 ? closely : 10;
            this.delay = delay;
            this.maxSize = maxSize ?? new Size(20, 20);
            this.number = number;
        }
        public virtual void Draw()
        {
            if (delay != 0) { return; }
            Game.Buffer.Graphics.DrawEllipse(Pens.White, pos.X, pos.Y, size.Width, size.Height);
            //Font font = new Font("Verdana", 10);
            //SolidBrush myBrush = new SolidBrush(Color.White);
            //Game.Buffer.Graphics.DrawString(number, font, myBrush, Pos.X + 1, Pos.Y + 1);
        }
        public virtual void Update()
        {
            if (delay != 0) { delay--; return; }

            int speedUp;
            
            int XSpeedUp = (int)(((double)Math.Abs(Game.startPoint.X - pos.X)) / Game.startPoint.X * 10);
                
            int YSpeedUp = (int)(((double)Math.Abs(Game.startPoint.Y - pos.Y)) / Game.startPoint.Y * 10);

            speedUp = XSpeedUp > YSpeedUp ? XSpeedUp : YSpeedUp;
            if (speedUp == 0) { speedUp = 1; }
            //if (XSpeedUp != 0 && XSpeedUp <= YSpeedUp) { speedUp = XSpeedUp; }
            //else if (YSpeedUp != 0 && YSpeedUp < XSpeedUp) { speedUp = YSpeedUp; }
            //else { speedUp = 1; }
            
                
            int maxDir = Math.Abs(dir.X) >= Math.Abs(dir.Y) ? Math.Abs(dir.X) : Math.Abs(dir.Y);
            if (maxDir == 0) { maxDir = 1; }
            int offsetX = dir.X * speedUp / maxDir;
            if (offsetX == 0) { offsetX = dir.X / Math.Abs(dir.X); }
            int offsetY = dir.Y * speedUp / maxDir;
            if (offsetY == 0) { offsetY = dir.Y / Math.Abs(dir.Y); }
            pos.X = pos.X + offsetX;
            pos.Y = pos.Y + offsetY;
            
            size.Width = (int)(maxSize.Width * ((double)Math.Abs(Game.startPoint.X - pos.X)) / Game.startPoint.X * (11 - closely));
            if (size.Width == 0) { size.Width = 1; }
            size.Height = (int)(maxSize.Height * ((double)Math.Abs(Game.startPoint.Y - pos.Y)) / Game.startPoint.Y * (11 - closely));
            if (size.Height == 0) { size.Height = 1; }
            if (size.Width >= size.Height) { size.Height = size.Width; }
            else { size.Width = size.Height; }
           
            if (pos.X + size.Width < 0 || pos.X - size.Width > Game.Width || pos.Y + size.Height < 0 || pos.Y - size.Height > Game.Height)
            {
                pos = Game.startPoint;
                size = new Size(1, 1);
            }
        }
    }

}
