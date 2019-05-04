using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    static class SpaceEngine
    {
        static int speedUp;
        static int XSpeedUp;
        static int YSpeedUp;
        static int maxDir;
        static int offsetX;
        static int offsetY;
        static int width;
        static int height;

        internal static void Update(BaseObject bObj)
        {
            XSpeedUp = (int)(((double)Math.Abs(Game.startPoint.X - bObj.pos.X)) / Game.startPoint.X * 10);
            YSpeedUp = (int)(((double)Math.Abs(Game.startPoint.Y - bObj.pos.Y)) / Game.startPoint.Y * 10);

            speedUp = XSpeedUp > YSpeedUp ? XSpeedUp : YSpeedUp;
            if (speedUp == 0) { speedUp = 1; }
            //if (XSpeedUp != 0 && XSpeedUp <= YSpeedUp) { speedUp = XSpeedUp; }
            //else if (YSpeedUp != 0 && YSpeedUp < XSpeedUp) { speedUp = YSpeedUp; }
            //else { speedUp = 1; }


            maxDir = Math.Abs(bObj.dir.X) >= Math.Abs(bObj.dir.Y) ? Math.Abs(bObj.dir.X) : Math.Abs(bObj.dir.Y);
            if (maxDir == 0) { maxDir = 1; }
            offsetX = bObj.dir.X * speedUp / maxDir;
            if (offsetX == 0) { offsetX = bObj.dir.X / Math.Abs(bObj.dir.X); }
            offsetY = bObj.dir.Y * speedUp / maxDir;
            if (offsetY == 0) { offsetY = bObj.dir.Y / Math.Abs(bObj.dir.Y); }
            bObj.pos = new Point(bObj.pos.X + offsetX, bObj.pos.Y + offsetY);

            width = (int)(bObj.maxSize.Width * ((double)Math.Abs(Game.startPoint.X - bObj.pos.X)) / Game.startPoint.X * (11 - bObj.closely));
            if (width == 0) { width = 1; }
            height = (int)(bObj.maxSize.Height * ((double)Math.Abs(Game.startPoint.Y - bObj.pos.Y)) / Game.startPoint.Y * (11 - bObj.closely));
            if (height == 0) { height = 1; }
            if (width >= height) { bObj.size = new Size(width, width); }
            else { bObj.size = new Size(height, height); }

            if (bObj.pos.X + width < 0 || bObj.pos.X - width > Game.Width || bObj.pos.Y + height < 0 || bObj.pos.Y - height > Game.Height)
            {
                bObj.pos = Game.startPoint;
                bObj.size = new Size(1, 1);
            }
        }
    }
}
