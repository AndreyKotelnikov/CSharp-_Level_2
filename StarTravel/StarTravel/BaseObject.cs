using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    abstract class BaseObject: ICollision
    {
        internal Point pos { get; set; }
        internal Point dir { get; set; }
        internal Size size { get; set; }
        internal Size maxSize { get; set; }
        internal int closely { get; set; } //1 - очень близко, 10 - очень далеко, 0 - летит прямо на тебя.
        protected Image image;
        internal int delay { get; set; }
        internal string text { get; set; }

        public BaseObject(Point pos, Point dir, Size size, int closely, Image image = null, int delay = 0, Size? maxSize = null, string text = "")
        {
            this.pos = pos;
            this.dir = dir;
            this.size = size;
            this.closely = closely <= 10 ? closely : 10;
            this.image = image;
            this.delay = delay;
            this.maxSize = maxSize ?? new Size(20, 20);
            this.text = text;
        }
        public virtual void Draw()
        {
            if (delay != 0) { return; }
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

        public Rectangle Rect => new Rectangle(pos, size);

    }

}
