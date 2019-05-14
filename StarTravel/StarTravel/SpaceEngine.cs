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
        /// <summary>
        /// Общий коэффициент повышения скорости движения объекта
        /// </summary>
        static int speedUp;
        /// <summary>
        /// Коэффициент повышения скорости движения объекта по оси X
        /// </summary>
        static int XSpeedUp;
        /// <summary>
        /// Коэффициент повышения скорости движения объекта по оси Y
        /// </summary>
        static int YSpeedUp;
        /// <summary>
        /// Максимальная величина смещения между смещением по оси X и оси Y
        /// </summary>
        static int maxDir;
        /// <summary>
        /// Смещение координат по оси X
        /// </summary>
        static int offsetX;
        /// <summary>
        /// Смещение координат по оси Y
        /// </summary>
        static int offsetY;
        /// <summary>
        /// Новая рассчётная ширина объекта
        /// </summary>
        static int width;
        /// <summary>
        /// Новая рассчётная высота объекта
        /// </summary>
        static int height;
        /// <summary>
        /// Точка, из которой (или в которую) движется объект
        /// </summary>
        static Point focusPoint;
        /// <summary>
        /// Семено для класса Random, чтобы избежать слипания объектов, 
        /// которые генерируют новую позицию практически одновременно
        /// </summary>
        static int seedForRandom;
        /// <summary>
        /// Обновляет позицию и размер объекта
        /// </summary>
        /// <param name="sObj">Объект, данные которого нужно обновить</param>
        internal static void Update(ISpaceMove sObj)
        {
            focusPoint = sObj.FocusPoint;

            if ((sObj as ICollision)?.KindOfCollisionObject == KindOfCollisionObject.DamageSpaceObject 
                || (sObj as ICollision)?.KindOfCollisionObject == KindOfCollisionObject.HealingSpaceObject
                || sObj is Boom)
            {
                DefineSpeedUpAsteroid(sObj);
                UpdatePosition(sObj);
                UpdateSizeAsteroid(sObj);
            }
            else if ((sObj as ICollision)?.KindOfCollisionObject == KindOfCollisionObject.Weapon)
            {
                DefineSpeedUp(sObj);
                UpdatePosition(sObj);
                UpdateSize(sObj);
            }
            else
            {
                DefineSpeedUp(sObj);
                UpdatePosition(sObj);
                UpdateSize(sObj);
            }
            CheckingOutOfScreen(sObj);
            //bObj.Text = speedUp.ToString();
        }

        //private static void UpdatePositionForCollision(BaseObject bObj)
        //{
        //    bObj.Pos = new Point(bObj.Pos.X < 0 ? (int)(-bObj.Size.Width * 0.1) : (bObj.Pos.X - bObj.Size.Width * speedUp / 50 - Game.Width / (Game.Width - bObj.Pos.X)),
        //        bObj.Pos.Y < 0 ? (int)(-bObj.Size.Height * 0.1) : (bObj.Pos.Y - bObj.Size.Height * speedUp / 50 - Game.Height / (Game.Height - bObj.Pos.Y)));
        //}

        /// <summary>
        /// Определяет общий коэффициент повышения скорости движения для объекта
        /// </summary>
        /// <param name="sObj">Объект, данные которого нужно обновить</param>
        private static void DefineSpeedUpAsteroid(ISpaceMove sObj)
        {
            XSpeedUp = (int)(Math.Abs((sObj.Dir.X > 0) ? ((double)sObj.Pos.X / (Game.Width - focusPoint.X)) : ((double)(focusPoint.X - sObj.Pos.X) / focusPoint.X)) * 10);
            YSpeedUp = (int)(Math.Abs((sObj.Dir.Y > 0) ? ((double)sObj.Pos.Y / (Game.Height - focusPoint.Y)) : ((double)(focusPoint.Y - sObj.Pos.Y) / focusPoint.Y)) * 10);

            speedUp = XSpeedUp > YSpeedUp ? XSpeedUp : YSpeedUp;
            if (speedUp == 0) { speedUp = 1; }
        }
        /// <summary>
        /// Проверяет выход объекта за рамки экрана и, в случае выхода, генерирует новую позицию объекта
        /// </summary>
        /// <param name="sObj">Объект, который нужно проверить</param>
        private static void CheckingOutOfScreen(ISpaceMove sObj)
        {
            if (sObj.Pos.X + width < 0 || sObj.Pos.X - width > Game.Width || sObj.Pos.Y + height < 0 || sObj.Pos.Y - height > Game.Height)
            {
               seedForRandom++;
               sObj.NewStartPosition(seedForRandom, 0);
            }
        }
        /// <summary>
        /// Обновляет размер объекта
        /// </summary>
        /// <param name="sObj">Объект, данные которого нужно обновить</param>
        private static void UpdateSize(ISpaceMove sObj)
        {
            width = (int)(sObj.MaxSize.Width * ((double)Math.Abs(focusPoint.X - sObj.Pos.X)) / focusPoint.X  * (11 - sObj.Closely));
            if (width == 0) { width = 1; }
            height = (int)(sObj.MaxSize.Height * ((double)Math.Abs(focusPoint.Y - sObj.Pos.Y)) / focusPoint.Y  * (11 - sObj.Closely));
            if (height == 0) { height = 1; }
            if (width >= height) { sObj.Size = new Size(width, width); }
            else { sObj.Size = new Size(height, height); }
        }
        /// <summary>
        /// Обновляет размер объекта
        /// </summary>
        /// <param name="sObj">Объект, данные которого нужно обновить</param>
        private static void UpdateSizeAsteroid(ISpaceMove sObj)
        {
            width = (int)(sObj.MaxSize.Width * (Math.Abs((sObj.Dir.X > 0) ? ((double)sObj.Pos.X / (Game.Width - focusPoint.X)) : ((double)(focusPoint.X - sObj.Pos.X) / focusPoint.X))  * (11 - sObj.Closely)));
            if (width == 0) { width = 1; }
            height = (int)(sObj.MaxSize.Height * (Math.Abs((sObj.Dir.Y > 0) ? ((double)sObj.Pos.Y / (Game.Height - focusPoint.Y)) : ((double)(focusPoint.Y - sObj.Pos.Y) / focusPoint.Y)) * (11 - sObj.Closely)));
            if (height == 0) { height = 1; }
            if (width >= height) { sObj.Size = new Size(width, width); }
            else { sObj.Size = new Size(height, height); }
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

        /// <summary>
        /// Обновляет позицию объекта
        /// </summary>
        /// <param name="sObj">Объект, данные которого нужно обновить</param>
        private static void UpdatePosition(ISpaceMove sObj)
        {
            maxDir = Math.Abs(sObj.Dir.X) >= Math.Abs(sObj.Dir.Y) ? Math.Abs(sObj.Dir.X) : Math.Abs(sObj.Dir.Y);
            if (maxDir == 0) { maxDir = 1; }
            offsetX = sObj.Dir.X * speedUp / maxDir;
            //offsetX = sObj.Dir.X * XSpeedUp / maxDir;
            if (offsetX == 0) { offsetX = sObj.Dir.X / Math.Abs(sObj.Dir.X); }
            offsetY = sObj.Dir.Y * speedUp / maxDir;
            //offsetY = sObj.Dir.Y * YSpeedUp / maxDir;
            if (offsetY == 0) { offsetY = sObj.Dir.Y / Math.Abs(sObj.Dir.Y); }
            sObj.Pos = new Point(sObj.Pos.X + offsetX, sObj.Pos.Y + offsetY);
        }

        /// <summary>
        /// Определяет общий коэффициент повышения скорости движения для объекта
        /// </summary>
        /// <param name="sObj">Объект, данные которого нужно обновить</param>
        private static void DefineSpeedUp(ISpaceMove sObj)
        {
            XSpeedUp = (int)(((double)Math.Abs(focusPoint.X - sObj.Pos.X)) / focusPoint.X * 10);
            YSpeedUp = (int)(((double)Math.Abs(focusPoint.Y - sObj.Pos.Y)) / focusPoint.Y * 10);

            speedUp = XSpeedUp > YSpeedUp ? XSpeedUp : YSpeedUp;
            if (speedUp == 0) { speedUp = 1; }
        }
    }
}
