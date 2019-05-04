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
                
                if (bObj.closely == 0)
                {
                    UpdateSizeAsteroidForCollision(bObj);
                    UpdatePositionForCollision(bObj);
                }
                else
                {
                    UpdateSizeAsteroid(bObj);
                    UpdatePosition(bObj);
                }
            }
            else if (bObj is Bullet)
            {
                pointOfCentre = (bObj as Bullet).FocusPoint;
                DefineSpeedUp(bObj);
                UpdatePosition(bObj);
                UpdateSize(bObj);
            }
            else
            {
                pointOfCentre = Game.startPoint;
                DefineSpeedUp(bObj);
                UpdatePosition(bObj);
                UpdateSize(bObj);
            }

            UpdatePosition(bObj);
            
            CheckingOutOfTheScreen(bObj);
            bObj.text = speedUp.ToString();
        }

        private static void UpdatePositionForCollision(BaseObject bObj)
        {
            bObj.pos = new Point(bObj.pos.X < 0 ? (int)(-bObj.size.Width * 0.1) : (bObj.pos.X - bObj.size.Width * speedUp / 50 - Game.Width / (Game.Width - bObj.pos.X)),
                bObj.pos.Y < 0 ? (int)(-bObj.size.Height * 0.1) : (bObj.pos.Y - bObj.size.Height * speedUp / 50 - Game.Height / (Game.Height - bObj.pos.Y)));
        }

        private static void DefineSpeedUpAsteroid(BaseObject bObj)
        {
            XSpeedUp = (int)(Math.Abs((bObj.dir.X > 0) ? ((double)bObj.pos.X / (Game.Width - pointOfCentre.X)) : ((double)(pointOfCentre.X - bObj.pos.X) / pointOfCentre.X)) * 10);
            YSpeedUp = (int)(Math.Abs((bObj.dir.Y > 0) ? ((double)bObj.pos.Y / (Game.Height - pointOfCentre.Y)) : ((double)(pointOfCentre.Y - bObj.pos.Y) / pointOfCentre.Y)) * 10);

            speedUp = XSpeedUp > YSpeedUp ? XSpeedUp : YSpeedUp;
            if (speedUp == 0) { speedUp = 1; }
        }

        private static void CheckingOutOfTheScreen(BaseObject bObj)
        {
            if (bObj.pos.X + width < 0 || bObj.pos.X - width > Game.Width || bObj.pos.Y + height < 0 || bObj.pos.Y - height > Game.Height)
            {
                if (bObj is Asteroid)
                {
                    Random rand = new Random();
                    Point startPoint = new Point(rand.Next(0, Game.Width), rand.Next(0, Game.Height));
                    bObj.pos = startPoint;
                    int size = rand.Next(1, 30);
                    bObj.maxSize = new Size(size, size);
                    (bObj as Asteroid).StartPoint = startPoint;
                    bObj.delay = rand.Next(1, 10);
                    bObj.closely = rand.Next(0, 1);
                    bObj.dir = new Point(Game.startPoint.X - bObj.pos.X >= 0 ? 1 : -1,
                        Game.startPoint.Y - bObj.pos.Y >= 0 ? 1 : -1);
                }
                else
                {
                    bObj.pos = pointOfCentre;
                }
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
            width = (int)(bObj.maxSize.Width * (Math.Abs((bObj.dir.X > 0) ? ((double)bObj.pos.X / (Game.Width - pointOfCentre.X)) : ((double)(pointOfCentre.X - bObj.pos.X) / pointOfCentre.X))  * (11 - bObj.closely)));
            if (width == 0) { width = 1; }
            height = (int)(bObj.maxSize.Height * (Math.Abs((bObj.dir.Y > 0) ? ((double)bObj.pos.Y / (Game.Height - pointOfCentre.Y)) : ((double)(pointOfCentre.Y - bObj.pos.Y) / pointOfCentre.Y)) * (11 - bObj.closely)));
            if (height == 0) { height = 1; }
            if (width >= height) { bObj.size = new Size(width, width); }
            else { bObj.size = new Size(height, height); }
        }

        private static void UpdateSizeAsteroidForCollision(BaseObject bObj)
        {
            width = (int)(bObj.maxSize.Width * (Math.Abs((bObj.dir.X > 0) ? ((double)bObj.pos.X / (Game.Width - pointOfCentre.X)) : ((double)(pointOfCentre.X - bObj.pos.X) / pointOfCentre.X)) * 20));
            if (width == 0) { width = 1; }
            height = (int)(bObj.maxSize.Height * (Math.Abs((bObj.dir.Y > 0) ? ((double)bObj.pos.Y / (Game.Height - pointOfCentre.Y)) : ((double)(pointOfCentre.Y - bObj.pos.Y) / pointOfCentre.Y)) * 20));
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
        }
    }
}
