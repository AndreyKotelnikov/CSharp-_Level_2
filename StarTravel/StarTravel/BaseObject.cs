using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    abstract class BaseObject: ICollision, IComparable
    {
        private static int countID;
        private Point pos;
        private Size size;

        internal int ID { get; private set; }
        internal int IsBoom { get; private set; }
        internal Boom Boom { get; set; }
        protected int boomMaxIndexImage;

        internal Point Pos
        {
            get { return pos; }
            set
            {
                if (value.X < -10000 || value.Y < -10000) { throw new GameObjectException("Координаты позиции не могут быть меньше -10000"); }
                else { pos = value; }
            }
        }
        internal Point Dir { get; set; }
        internal Size Size
        {
            get { return size; }
            set
            {
                if (value.Width < 0 || value.Height < 0) { throw new GameObjectException("Размер не может быть меньше нуля"); }
                else { size = value; }
            }
        }
        internal Size MaxSize { get; set; }
        internal int Closely { get; set; } //0 - очень близко, 10 - очень далеко
        protected Image image;
        internal int Delay { get; set; }
        internal string Text { get; set; }
        public Rectangle Rect { get { return new Rectangle(Pos, Size); } }

        static BaseObject()
        {
            countID = 0;
        }

        public BaseObject(Point pos, Point dir, Size size, int closely, Image image = null, int delay = 0, Size? maxSize = null, string text = "")
        {
            ID = countID;
            countID++;
            Pos = pos;
            Dir = dir;
            Size = size;
            Closely = closely <= 10 ? closely : 10;
            this.image = image;
            Delay = delay;
            MaxSize = maxSize ?? new Size(20, 20);
            Text = text;
        }
        public virtual void Draw()
        {
            //if (delay != 0) { return; }
            //Game.Buffer.Graphics.DrawEllipse(Pens.White, pos.X, pos.Y, size.Width, size.Height);
            //Font font = new Font("Verdana", (int)(size.Width * 0.9) >= 1 ? (int)(size.Width * 0.9) : 1);
            //SolidBrush myBrush = new SolidBrush(Color.White);
            //Game.Buffer.Graphics.DrawString(text, font, myBrush, pos.X + 1, pos.Y + 1);
        }
        public abstract void Update();
        

        public bool Collision(List<Bullet> o)
        {
            foreach (var item in o)
            {
                if (item.Rect.IntersectsWith(Rect)) { return true; }
            }
            return false;
        }

        public int CompareTo(object obj)
        {
            if (this is Bullet && (obj is Asteroid || obj is Star)) { return 1; }
            if (this is Asteroid && obj is Bullet) { return -1; }
            if (this is Asteroid && obj is Star) { return 1; }
            if (this is Star && (obj is Asteroid || obj is Bullet)) { return -1; }
            if (Closely < (obj as BaseObject).Closely) { return 1; }
            else if (Closely > (obj as BaseObject).Closely) { return -1; }
            if (Size.Width > (obj as BaseObject).Size.Width) { return 1; }
            else if (Size.Width < (obj as BaseObject).Size.Width) { return -1; }
            return 0;
        }
    }

}
