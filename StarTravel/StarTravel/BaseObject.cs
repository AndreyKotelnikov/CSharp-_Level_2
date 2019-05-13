using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarTravel
{
    /// <summary>
    /// Базовый абстрактный класс для космических объектов
    /// </summary>
    abstract class BaseObject: ICollision, ISpaceMove, IDraw, IUpdate, IBoom, ILog
    {
        protected static int seedForRandom = 0;
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

        protected bool isBoom;

        /// <summary>
        /// ID объекта
        /// </summary>
        internal int ID { get; private set; }

        public KindOfCollisionObject KindOfCollisionObject { get; private set; }

        public int DrawingPriority { get; private set; }
        /// <summary>
        /// Объект взрывается сейчас?
        /// </summary>
        public virtual bool IsBoom
        {
            get => isBoom;
            protected set
            {
                if (isBoom == false && value == true)
                {
                    LogEventArgs e = new LogEventArgs(this, null, GetType().GetProperty("IsBoom"), message: "взорвался");
                    Logging?.Invoke(this, e);
                }
                isBoom = value;
            }
        }
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
        public virtual Rectangle Rect { get { return new Rectangle(new Point(Pos.X - Size.Width/2, Pos.Y - Size.Height/2), Size); } }

        public event EventHandler<LogEventArgs> Logging;

        static BaseObject()
        {
            countID = 0;
        }

        public BaseObject(Point pos, Point dir, Size size, int closely, int drawingPriority, 
            KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.NoDamageSpaceObject, Image image = null, 
            Point? focusPoint = null, int delay = 0, Size? maxSize = null, string text = "")
        {
            ID = countID;
            countID++;
            Pos = pos;
            Dir = dir;
            Size = size;
            Closely = closely <= 10 ? closely : 10;
            DrawingPriority = drawingPriority;
            KindOfCollisionObject = kindOfCollisionObject;
            this.image = image;
            FocusPoint = focusPoint?? Game.ScreenCenterPoint;
            Delay = delay;
            MaxSize = maxSize ?? new Size(20, 20);
            Text = text;
            IsBoom = false;
            
        }
        public virtual void Draw()
        {
            if (KindOfCollisionObject == KindOfCollisionObject.DamageSpaceObject && Closely == 0)
            {
                Pen pen = new Pen(Color.Red, 2);
                Game.Buffer.Graphics.DrawEllipse(pen, Rect);
            }
            if (Text != string.Empty)
            {
                double offSet = 0.3;
                Font font = new Font("Verdana", (int)(Size.Width * offSet) >= 1 ? (int)(Size.Width * offSet) : 1);
                SolidBrush myBrush = new SolidBrush(Color.White);
                Game.Buffer.Graphics.DrawString(Text, font, myBrush, Pos.X - Size.Width/2, Pos.Y - Size.Height/2 + (int)(Size.Height * (1 - offSet)));
            }
        }
        
        public abstract void Update();
        

        public virtual bool Collision(IDraw[] o)
        {
            foreach (var item in o)
            {
                if ( item is ICollision && (item as ICollision).KindOfCollisionObject == KindOfCollisionObject.Weapon 
                    && (item as ICollision).Rect.IntersectsWith(Rect) && (item as Bullet).TimeOfShooting > 0)
                { return true; }
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
            if (KindOfCollisionObject == KindOfCollisionObject.DamageSpaceObject && Closely == 0 && IsBoom == false)
            {
                Random rand = new Random();
                int damage = rand.Next(MaxSize.Width / 2, MaxSize.Width);
                LogEventArgs logEvent = new LogEventArgs(this, GetType().GetMethod("NewStartPosition"), null, Game.Ship, damage, "нанёс урон");
                Logging?.Invoke(this, logEvent);
                Game.Ship.EnergyLow(damage);
                System.Media.SystemSounds.Asterisk.Play();
                
            }
            IsBoom = false;
            Boom = null;
        }

        public virtual void Game_KillAsteroid(int killAsteroids)
        {
            Text = $"Ты убил {killAsteroids} {Helper.InflectionOfWord(killAsteroids, "астероид", "астероида", "астероидов")}";
        }

        protected void StartLogginEvent(Object o, LogEventArgs logEvent)
        {
            Logging?.Invoke(0, logEvent);
        }
        

    }

}
