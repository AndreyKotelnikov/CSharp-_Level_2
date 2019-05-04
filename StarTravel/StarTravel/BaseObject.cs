using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class BaseObject
    {
        internal Point pos { get; set; }
        internal Point dir { get; }
        internal Size size { get; set; }
        internal Size maxSize { get; }
        internal int closely { get; } //0 - очень близко, 10 - очень далеко
        internal int delay;
        internal string number { get; }

        internal Point Pos { get; set; }

        public BaseObject(Point pos, Point dir, Size size, int closely, int delay = 0, Size? maxSize = null, string number = "")
        {
            this.pos = pos;
            this.dir = dir;
            this.size = size;
            this.closely = closely <= 10 ? closely : 10;
            this.delay = delay;
            this.maxSize = maxSize ?? new Size(20, 20);
            this.number = number;
        }
        public virtual void Draw()
        {
            if (delay != 0) { return; }
            Game.Buffer.Graphics.DrawEllipse(Pens.White, pos.X, pos.Y, size.Width, size.Height);
            //Font font = new Font("Verdana", 10);
            //SolidBrush myBrush = new SolidBrush(Color.White);
            //Game.Buffer.Graphics.DrawString(number, font, myBrush, Pos.X + 1, Pos.Y + 1);
        }
        public virtual void Update()
        {
            if (delay != 0) { delay--; return; }
            SpaceEngine.Update(this);
        }
    }

}
