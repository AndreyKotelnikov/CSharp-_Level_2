using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class Asteroid : BaseObject
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
        /// <param name="focusPoint"></param>
        /// <param name="kindOfCollisionObject"></param>
        /// <param name="drawingPriority"></param>
        /// <param name="maxSize"></param>
        /// <param name="text"></param>
        public Asteroid(Point pos, Point dir, Size size, int closely, int delay, Image image, 
            Point focusPoint, KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.DamageSpaceObject, 
            int drawingPriority = 5, Size? maxSize = null, string text = "")
            : base(pos, dir, size, closely, drawingPriority, kindOfCollisionObject, image, focusPoint, delay, maxSize, text)
        {
            FocusPoint = focusPoint;
            Boom = null;
            Text = Closely == 0 ? "Летит в корабль" : "";
        }
        /// <summary>
        /// Переопределённый метод обновления данных объекта
        /// </summary>
        public override void Update()
        {
            if (IsBoom)
            {
                if (Boom.EndBoom)
                {
                    NewStartPosition(seedForRandom, 100);
                    seedForRandom++;
                }
                else { Boom.Update(); }
            }
            else
            {
                if (Delay != 0) { Delay--; return; }
                SpaceEngine.Update(this);
            }
        }
        /// <summary>
        /// Переопределённый метод отрисовки объекта на форме
        /// </summary>
        public override void Draw()
        {
            if (Boom != null) { Boom.Draw(); }
            else
            {
                if (Delay != 0) { return; }
                Game.Buffer.Graphics.DrawImage(image, Pos.X - Size.Width/2, Pos.Y - Size.Height/2, Size.Width, Size.Height);
                base.Draw();
            }
        }
        /// <summary>
        /// Генерирует новую случайную позицию объекта 
        /// </summary>
        /// <param name="seedForRandom">Семено для класса Random, чтобы избежать слипания объектов, 
        /// которые генерируют новую позицию практически одновременно</param>
        /// <param name="delay">Задержка отрисовки объекта на определённое количество попыток его отрисовки</param>
        public override void NewStartPosition(int seedForRandom = 0, int delay = 0)
        {
            base.NewStartPosition();
            Random rand;
            if (seedForRandom == 0) { rand = new Random(); } else { rand = new Random(seedForRandom); }
            Point startPoint = new Point(rand.Next(0, Game.Width), rand.Next(0, Game.Height));
            Pos = startPoint;
            Size = new Size(1, 1);
            FocusPoint = startPoint;
            int size = rand.Next(1, 30);
            MaxSize = new Size(size, size);
            if (delay == 0) { Delay = rand.Next(1, 100); } else { Delay = delay; }
            Closely = rand.Next(0, 3);
            Text = Closely == 0 ? "Летит в корабль" : "";
            Dir = new Point(Game.ScreenCenterPoint.X - Pos.X >= 0 ? rand.Next(1, 5) : -rand.Next(1, 5),
                Game.ScreenCenterPoint.Y - Pos.Y >= 0 ? rand.Next(1, 5) : -rand.Next(1, 5));
        }
    }
}
