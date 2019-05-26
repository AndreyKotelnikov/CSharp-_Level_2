using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Аптечка первой помощи
    /// </summary>
    class FirstAidKit : Asteroid
    {
        /// <summary>
        /// Переопределённое свойство "Объект взорвался?"
        /// </summary>
        public override bool IsBoom
        {
            get => isBoom;
            protected set
            {
                if (isBoom == false && value == true)
                {
                    Random rand = new Random();
                    int cure = rand.Next(10 - MaxSize.Width > 0 ? 10 - MaxSize.Width : 1, (15 - MaxSize.Width > 0 ? 15 - MaxSize.Width : 1) * 2);
                    Game.Ship.EnergyUp(cure);
                    LogEventArgs e = new LogEventArgs(this, null, GetType().GetProperty("IsBoom"), Game.Ship, cure, "вылечила жизни на");
                    StartLogginEvent(this, e);
                }
                isBoom = value;
            }
        }

        /// <summary>
        /// Конструктор для экземпляра класса
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
        public FirstAidKit(Point pos, Point dir, Size size, int closely, int delay, Image image,
            Point focusPoint, KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.HealingSpaceObject,
            int drawingPriority = 5, Size? maxSize = null, string text = "") : base(pos, dir, size, closely, delay, image,
             focusPoint, kindOfCollisionObject, drawingPriority, maxSize, text)
        {
            Text = string.Empty;
        }

        /// <summary>
        /// Переопределённый метод обновления данных объекта
        /// </summary>
        public override void Update()
        {
            if (IsBoom)
            {
                NewStartPosition(seedForRandom, 100);
                seedForRandom++;
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
            if(Boom != null) { Boom = null; }
            base.Draw();
            Pen pen = new Pen(Color.Green, 2);
            Game.Buffer.Graphics.DrawEllipse(pen, Rect);
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
            Random rand = new Random();
            Text = string.Empty;
            Delay = rand.Next(200, 400);
            int size = rand.Next(1, 10);
            MaxSize = new Size(size, size);
            if(Closely == 0) { Closely = rand.Next(1, 11); }
        }

        public override void CreatBoom(Image[] images = null, int repeatEveryImage = 0)
        {
            IsBoom = true;
        }
    }
}
