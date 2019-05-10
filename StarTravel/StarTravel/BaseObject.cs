using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Базовый абстрактный класс для космических объектов
    /// </summary>
    abstract class BaseObject: ICollision, ISpaceMove, IDraw, IUpdate, IBoom
    {
        /// <summary>
        /// ID объекта
        /// </summary>
        private static int countID;
        /// <summary>
        /// Текущая позиция объекта
        /// </summary>
        private Point pos;

        /// <summary>
        /// Текущий размер объекта
        /// </summary>
        private Size size;
        
        /// <summary>
        /// ID объекта
        /// </summary>
        internal int ID { get; private set; }

        public int DrawingPriority { get; private set; }
        /// <summary>
        /// Объект взрывается сейчас?
        /// </summary>
        public bool IsBoom { get; private set; }
        /// <summary>
        /// Ссылка на объект взрыва
        /// </summary>
        public Boom Boom { get; protected set; }
        /// <summary>
        /// Точка фокусировки объекта в космосе (объект летит из неё или в неё)
        /// </summary>
        public Point FocusPoint { get; protected set; }
        /// <summary>
        /// Текущая позиция объекта
        /// </summary>
        public Point Pos
        {
            get { return pos; }
            set
            {
                if (value.X < -10000 || value.Y < -10000) { throw new GameObjectException("Координаты позиции не могут быть меньше -10000"); }
                else { pos = value; }
            }
        }
        /// <summary>
        /// Текущая директория движения объекта
        /// </summary>
        public Point Dir { get; protected set; }
        /// <summary>
        /// Текущий размер объекта
        /// </summary>
        public Size Size
        {
            get { return size; }
            set
            {
                if (value.Width < 0 || value.Height < 0) { throw new GameObjectException("Размер не может быть меньше нуля"); }
                else { size = value; }
            }
        }
        /// <summary>
        /// Максимальный размер объекта
        /// </summary>
        public Size MaxSize { get; protected set; }
        /// <summary>
        /// Близость траектории движения объекта к кораблю (0 - очень близко, 10 - очень далеко)
        /// </summary>
        public int Closely { get; protected set; }
        /// <summary>
        /// Картинка, которой отображается объект
        /// </summary>
        protected Image image;
        /// <summary>
        /// Задержка на количество попыток отрисовки объекта до начала его отрисовки
        /// </summary>
        internal int Delay { get; set; }
        /// <summary>
        /// Текст, который отображается на объекте
        /// </summary>
        internal string Text { get; set; }
        /// <summary>
        /// Площадь объекта на форме
        /// </summary>
        public Rectangle Rect { get { return new Rectangle(Pos, Size); } }
                
        static BaseObject()
        {
            countID = 0;
        }

        public BaseObject(Point pos, Point dir, Size size, int closely, int drawingPriority, Image image = null, Point? focusPoint = null, int delay = 0, Size? maxSize = null, string text = "")
        {
            ID = countID;
            countID++;
            Pos = pos;
            Dir = dir;
            Size = size;
            Closely = closely <= 10 ? closely : 10;
            DrawingPriority = drawingPriority;
            this.image = image;
            FocusPoint = focusPoint?? Game.StartPoint;
            Delay = delay;
            MaxSize = maxSize ?? new Size(20, 20);
            Text = text;
            IsBoom = false;
            
        }
        public abstract void Draw();
        //{
            //if (delay != 0) { return; }
            //Game.Buffer.Graphics.DrawEllipse(Pens.White, pos.X, pos.Y, size.Width, size.Height);
            //Font font = new Font("Verdana", (int)(size.Width * 0.9) >= 1 ? (int)(size.Width * 0.9) : 1);
            //SolidBrush myBrush = new SolidBrush(Color.White);
            //Game.Buffer.Graphics.DrawString(text, font, myBrush, pos.X + 1, pos.Y + 1);
        //}
        public abstract void Update();
        

        public bool Collision(List<Bullet> o)
        {
            foreach (var item in o)
            {
                if (item.Rect.IntersectsWith(Rect)) { return true; }
            }
            return false;
        }

        //public int CompareTo(object obj)
        //{
        //    if (this is Bullet && (obj is Asteroid || obj is Star)) { return 1; }
        //    if (this is Asteroid && obj is Bullet) { return -1; }
        //    if (this is Asteroid && obj is Star) { return 1; }
        //    if (this is Star && (obj is Asteroid || obj is Bullet)) { return -1; }
        //    if (Closely < (obj as BaseObject).Closely) { return 1; }
        //    else if (Closely > (obj as BaseObject).Closely) { return -1; }
        //    if (Size.Width > (obj as BaseObject).Size.Width) { return 1; }
        //    else if (Size.Width < (obj as BaseObject).Size.Width) { return -1; }
        //    return 0;
        //}

        public void CreatBoom(Image[] images, int repeatEveryImage = 2)
        {
            IsBoom = true;
            Boom = new Boom(Pos, Dir, Size, Closely, images, FocusPoint, repeatEveryImage, DrawingPriority);
        }

        public virtual void NewStartPosition(int seedForRandom = 0, int delay = 0)
        {
            IsBoom = false;
            Boom = null;
        }
    }

}
