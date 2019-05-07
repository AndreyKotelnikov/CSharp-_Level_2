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
        internal static List<Asteroid> AsteroidsList { get; set; }
        
        

        static Asteroid()
        {
            AsteroidsList = new List<Asteroid>();
        }

        public Asteroid(Point pos, Point dir, Size size, int closely, int delay, Image image, 
            Point focusPoint, Size? maxSize = null, string text = "")
            : base(pos, dir, size, closely, image, focusPoint, delay, maxSize, text)
        {
            AsteroidsList.Add(this);
            FocusPoint = focusPoint;
            Boom = null;
        }

        

        public override void Update()
        {
            if (IsBoom)
            {
                if (Boom.EndBoom)
                {
                    NewStartPosition(0, 100);
                }
                else { Boom.Update(); }
            }
            else
            {
                if (Delay != 0) { Delay--; return; }
                SpaceEngine.Update(this);
            }
        }

        public override void Draw()
        {
            if (Boom != null) { Boom.Draw(); }
            else
            {
                if (Delay != 0) { return; }
                Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, Size.Width, Size.Height);
            }
            //Font font = new Font("Verdana", (int)(size.Width * 0.9) >= 1 ? (int)(size.Width * 0.9) : 1);
            //SolidBrush myBrush = new SolidBrush(Color.White);
            //Game.Buffer.Graphics.DrawString(text, font, myBrush, pos.X + 1, pos.Y + 1);
        }

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
            Closely = rand.Next(0, 10);
            Dir = new Point(Game.startPoint.X - Pos.X >= 0 ? rand.Next(1, 5) : -rand.Next(1, 5),
                Game.startPoint.Y - Pos.Y >= 0 ? rand.Next(1, 5) : -rand.Next(1, 5));
        }
    }
}
