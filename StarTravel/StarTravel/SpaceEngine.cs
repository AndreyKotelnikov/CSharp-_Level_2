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
        static Point pointOfCentre;

        internal static void Update(BaseObject bObj)
        {
            if (bObj is Asteroid)
            {
                pointOfCentre = (bObj as Asteroid).StartPoint;
                
                DefineSpeedUpAsteroid(bObj);
                UpdateSizeAsteroid(bObj);
            }
            else if (bObj is Bullet)
            {
                pointOfCentre = (bObj as Bullet).FocusPoint;
                DefineSpeedUp(bObj);
                UpdateSize(bObj);
            }
            else
            {
                pointOfCentre = Game.startPoint;
                DefineSpeedUp(bObj);
                UpdateSize(bObj);
            }

            UpdatePosition(bObj);
            
            CheckingOutOfTheScreen(bObj);
            bObj.text = speedUp.ToString();
        }

        private static void DefineSpeedUpAsteroid(BaseObject bObj)
        {
            XSpeedUp = (int)((double)Math.Abs(bObj.dir.X > 0 ? bObj.pos.X : Game.Width - pointOfCentre.X - bObj.pos.X) / (Game.Width - pointOfCentre.X) * 10);
            YSpeedUp = (int)((double)Math.Abs(bObj.dir.Y > 0 ? bObj.pos.Y : Game.Height - pointOfCentre.Y - bObj.pos.Y) / (Game.Height - pointOfCentre.Y) * 10);

            speedUp = XSpeedUp > YSpeedUp ? XSpeedUp : YSpeedUp;
            if (speedUp == 0) { speedUp = 1; }
        }

        private static void CheckingOutOfTheScreen(BaseObject bObj)
        {
            if (bObj.pos.X + width < 0 || bObj.pos.X - width > Game.Width || bObj.pos.Y + height < 0 || bObj.pos.Y - height > Game.Height)
            {
                bObj.pos = pointOfCentre;
                bObj.size = new Size(1, 1);
            }
        }

        private static void UpdateSize(BaseObject bObj)
        {
            width = (int)(bObj.maxSize.Width * ((double)Math.Abs(pointOfCentre.X - bObj.pos.X)) / pointOfCentre.X  * (11 - bObj.closely));
            if (width == 0) { width = 1; }
            height = (int)(bObj.maxSize.Height * ((double)Math.Abs(pointOfCentre.Y - bObj.pos.Y)) / pointOfCentre.Y  * (11 - bObj.closely));
            if (height == 0) { height = 1; }
            if (width >= height) { bObj.size = new Size(width, width); }
            else { bObj.size = new Size(height, height); }
        }

        private static void UpdateSizeAsteroid(BaseObject bObj)
        {
            width = (int)(bObj.maxSize.Width * ((double)Math.Abs(bObj.dir.X > 0 ? bObj.pos.X : Game.Width - pointOfCentre.X - bObj.pos.X) / (Game.Width - pointOfCentre.X) * (11 - bObj.closely)));
            if (width == 0) { width = 1; }
            height = (int)(bObj.maxSize.Height * ((double)Math.Abs(bObj.dir.Y > 0 ? bObj.pos.Y : Game.Height - pointOfCentre.Y - bObj.pos.Y) / (Game.Height - pointOfCentre.Y) * (11 - bObj.closely)));
            if (height == 0) { height = 1; }
            if (width >= height) { bObj.size = new Size(width, width); }
            else { bObj.size = new Size(height, height); }
        }

        private static void UpdatePosition(BaseObject bObj)
        {
            maxDir = Math.Abs(bObj.dir.X) >= Math.Abs(bObj.dir.Y) ? Math.Abs(bObj.dir.X) : Math.Abs(bObj.dir.Y);
            if (maxDir == 0) { maxDir = 1; }
            offsetX = bObj.dir.X * speedUp / maxDir;
            if (offsetX == 0) { offsetX = bObj.dir.X / Math.Abs(bObj.dir.X); }
            offsetY = bObj.dir.Y * speedUp / maxDir;
            if (offsetY == 0) { offsetY = bObj.dir.Y / Math.Abs(bObj.dir.Y); }
            bObj.pos = new Point(bObj.pos.X + offsetX, bObj.pos.Y + offsetY);
        }

        private static void DefineSpeedUp(BaseObject bObj)
        {
            XSpeedUp = (int)(((double)Math.Abs(pointOfCentre.X - bObj.pos.X)) / pointOfCentre.X * 10);
            YSpeedUp = (int)(((double)Math.Abs(pointOfCentre.Y - bObj.pos.Y)) / pointOfCentre.Y * 10);

            speedUp = XSpeedUp > YSpeedUp ? XSpeedUp : YSpeedUp;
            if (speedUp == 0) { speedUp = 1; }
            //if (XSpeedUp != 0 && XSpeedUp <= YSpeedUp) { speedUp = XSpeedUp; }
            //else if (YSpeedUp != 0 && YSpeedUp < XSpeedUp) { speedUp = YSpeedUp; }
            //else { speedUp = 1; }
        }
    }
}
