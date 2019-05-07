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
        static int count;

        internal static void Update(BaseObject bObj)
        {
            if (bObj is Asteroid || bObj is Boom)
            {
                pointOfCentre = (bObj as Asteroid).StartPoint;
                
                DefineSpeedUpAsteroid(bObj);

                //if (bObj.closely == 0)
                //{
                //    UpdateSizeAsteroidForCollision(bObj);
                //    UpdatePositionForCollision(bObj);
                //}
                //else
                //{
                    UpdateSizeAsteroid(bObj);
                    UpdatePosition(bObj);
                //}
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
            
            CheckingOutOfScreen(bObj);
            //bObj.Text = speedUp.ToString();
        }

        //private static void UpdatePositionForCollision(BaseObject bObj)
        //{
        //    bObj.Pos = new Point(bObj.Pos.X < 0 ? (int)(-bObj.Size.Width * 0.1) : (bObj.Pos.X - bObj.Size.Width * speedUp / 50 - Game.Width / (Game.Width - bObj.Pos.X)),
        //        bObj.Pos.Y < 0 ? (int)(-bObj.Size.Height * 0.1) : (bObj.Pos.Y - bObj.Size.Height * speedUp / 50 - Game.Height / (Game.Height - bObj.Pos.Y)));
        //}

        private static void DefineSpeedUpAsteroid(BaseObject bObj)
        {
            XSpeedUp = (int)(Math.Abs((bObj.Dir.X > 0) ? ((double)bObj.Pos.X / (Game.Width - pointOfCentre.X)) : ((double)(pointOfCentre.X - bObj.Pos.X) / pointOfCentre.X)) * 10);
            YSpeedUp = (int)(Math.Abs((bObj.Dir.Y > 0) ? ((double)bObj.Pos.Y / (Game.Height - pointOfCentre.Y)) : ((double)(pointOfCentre.Y - bObj.Pos.Y) / pointOfCentre.Y)) * 10);

            speedUp = XSpeedUp > YSpeedUp ? XSpeedUp : YSpeedUp;
            if (speedUp == 0) { speedUp = 1; }
        }

        private static void CheckingOutOfScreen(BaseObject bObj)
        {
            if (bObj.Pos.X + width < 0 || bObj.Pos.X - width > Game.Width || bObj.Pos.Y + height < 0 || bObj.Pos.Y - height > Game.Height)
            {
                if (bObj is Asteroid)
                {
                    count++;
                    (bObj as Asteroid).NewStartPosition(count);
                }
                else
                {
                    bObj.Pos = pointOfCentre;
                }
                bObj.Size = new Size(1, 1);
            }
        }

        private static void UpdateSize(BaseObject bObj)
        {
            width = (int)(bObj.MaxSize.Width * ((double)Math.Abs(pointOfCentre.X - bObj.Pos.X)) / pointOfCentre.X  * (11 - bObj.Closely));
            if (width == 0) { width = 1; }
            height = (int)(bObj.MaxSize.Height * ((double)Math.Abs(pointOfCentre.Y - bObj.Pos.Y)) / pointOfCentre.Y  * (11 - bObj.Closely));
            if (height == 0) { height = 1; }
            if (width >= height) { bObj.Size = new Size(width, width); }
            else { bObj.Size = new Size(height, height); }
        }

        private static void UpdateSizeAsteroid(BaseObject bObj)
        {
            width = (int)(bObj.MaxSize.Width * (Math.Abs((bObj.Dir.X > 0) ? ((double)bObj.Pos.X / (Game.Width - pointOfCentre.X)) : ((double)(pointOfCentre.X - bObj.Pos.X) / pointOfCentre.X))  * (11 - bObj.Closely)));
            if (width == 0) { width = 1; }
            height = (int)(bObj.MaxSize.Height * (Math.Abs((bObj.Dir.Y > 0) ? ((double)bObj.Pos.Y / (Game.Height - pointOfCentre.Y)) : ((double)(pointOfCentre.Y - bObj.Pos.Y) / pointOfCentre.Y)) * (11 - bObj.Closely)));
            if (height == 0) { height = 1; }
            if (width >= height) { bObj.Size = new Size(width, width); }
            else { bObj.Size = new Size(height, height); }
        }

        //private static void UpdateSizeAsteroidForCollision(BaseObject bObj)
        //{
        //    width = (int)(bObj.MaxSize.Width * (Math.Abs((bObj.Dir.X > 0) ? ((double)bObj.Pos.X / (Game.Width - pointOfCentre.X)) : ((double)(pointOfCentre.X - bObj.Pos.X) / pointOfCentre.X)) * 10));
        //    if (width == 0) { width = 1; }
        //    height = (int)(bObj.MaxSize.Height * (Math.Abs((bObj.Dir.Y > 0) ? ((double)bObj.Pos.Y / (Game.Height - pointOfCentre.Y)) : ((double)(pointOfCentre.Y - bObj.Pos.Y) / pointOfCentre.Y)) * 10));
        //    if (height == 0) { height = 1; }
        //    if (width >= height) { bObj.Size = new Size(width, width); }
        //    else { bObj.Size = new Size(height, height); }
        //}

        private static void UpdatePosition(BaseObject bObj)
        {
            maxDir = Math.Abs(bObj.Dir.X) >= Math.Abs(bObj.Dir.Y) ? Math.Abs(bObj.Dir.X) : Math.Abs(bObj.Dir.Y);
            if (maxDir == 0) { maxDir = 1; }
            offsetX = bObj.Dir.X * speedUp / maxDir;
            if (offsetX == 0) { offsetX = bObj.Dir.X / Math.Abs(bObj.Dir.X); }
            offsetY = bObj.Dir.Y * speedUp / maxDir;
            if (offsetY == 0) { offsetY = bObj.Dir.Y / Math.Abs(bObj.Dir.Y); }
            bObj.Pos = new Point(bObj.Pos.X + offsetX, bObj.Pos.Y + offsetY);
        }

        private static void DefineSpeedUp(BaseObject bObj)
        {
            XSpeedUp = (int)(((double)Math.Abs(pointOfCentre.X - bObj.Pos.X)) / pointOfCentre.X * 10);
            YSpeedUp = (int)(((double)Math.Abs(pointOfCentre.Y - bObj.Pos.Y)) / pointOfCentre.Y * 10);

            speedUp = XSpeedUp > YSpeedUp ? XSpeedUp : YSpeedUp;
            if (speedUp == 0) { speedUp = 1; }
        }
    }
}
