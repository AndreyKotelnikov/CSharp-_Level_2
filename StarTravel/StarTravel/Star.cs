using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class Star:BaseObject
    {
        /// <summary>
        /// Конструктор экземпляра класса
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="size"></param>
        /// <param name="closely"></param>
        /// <param name="delay"></param>
        /// <param name="image"></param>
        /// <param name="kindOfCollisionObject"></param>
        /// <param name="drawingPriority"></param>
        /// <param name="focusPoint"></param>
        /// <param name="maxSize"></param>
        /// <param name="text"></param>
        public Star(Point pos, Point dir, Size size, int closely, int delay, Image image, 
            KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.NoDamageSpaceObject, int drawingPriority = 10, Point? focusPoint = null, Size? maxSize = null, string text = "") 
            : base(pos, dir, size, closely, drawingPriority, kindOfCollisionObject, image, focusPoint, delay, maxSize, text)
        {
            
        }

        /// <summary>
        /// Переопределённый метод отрисовки объекта на форме
        /// </summary>
        public override void Draw()
        {
            //Game.Buffer.Graphics.DrawLine(Pens.White, pos.X, pos.Y, pos.X + size.Width, pos.Y + size.Height);
            //Game.Buffer.Graphics.DrawLine(Pens.White, pos.X + size.Width, pos.Y, pos.X, pos.Y + size.Height);

            if (Delay != 0) { return; }
            Game.Buffer.Graphics.DrawImage(image, Pos.X - Size.Width / 2, Pos.Y - Size.Height / 2, Size.Width, Size.Height);
            base.Draw();
            

            //Game.Buffer.Graphics.ResetTransform();

            //using (GraphicsPath path = new GraphicsPath())
            //using (Brush brush = new TextureBrush(image))
            //using (Brush brush = new TextureBrush(image, new Rectangle(pos.X, pos.Y, 200, 200))) 
            //{
            //    path.AddPie(pos.X, pos.Y, size.Width, size.Height, 0, 360);
            //    Game.Buffer.Graphics.FillPath(brush, path);
            //}
        }
        /// <summary>
        /// Переопределённый метод обновления данных объекта
        /// </summary>
        public override void Update()
        {
            if (Delay != 0) { Delay--; return; }
            SpaceEngine.Update(this);
        }

        /// <summary>
        /// Генерирует новую случайную позицию объекта 
        /// </summary>
        /// <param name="seedForRandom">Семено для класса Random, чтобы избежать слипания объектов, 
        /// которые генерируют новую позицию практически одновременно</param>
        /// <param name="delay">Задержка отрисовки объекта на определённое количество попыток его отрисовки</param>
        public override void NewStartPosition(int seedForRandom = 0, int delay = 0)
        {
            Pos = Game.ScreenCenterPoint;
            Size = new Size(1, 1);
        }

    }
}
