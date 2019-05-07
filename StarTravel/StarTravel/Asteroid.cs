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
        internal Point StartPoint { get; set; }
        

        static Asteroid()
        {
            AsteroidsList = new List<Asteroid>();
        }

        public Asteroid(Point pos, Point dir, Size size, int closely, int delay, Image image, 
            Point startPoint, Size? maxSize = null, string text = "")
            : base(pos, dir, size, closely, image, delay, maxSize, text)
        {
            AsteroidsList.Add(this);
            StartPoint = startPoint;
            Boom = null;
        }

        internal void CreatBoom(Image[] images)
        {
            boomMaxIndexImage = images.Length - 1;
            Boom = new Boom(Pos, Dir, Size, Closely, images, StartPoint);
        }

        public override void Update()
        {
            if (Boom != null && Boom.IndexImage >= boomMaxIndexImage)
            {
                Boom = null;
                NewAsteroid(0, 100);
            }
            if (Boom != null) { Boom.Update(); }
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

        public void NewAsteroid(int seed = 0, int delay = 0)
        {
            Random rand;
            if (seed == 0) { rand = new Random(); } else { rand = new Random(seed); }
            Point startPoint = new Point(rand.Next(0, Game.Width), rand.Next(0, Game.Height));
            Pos = startPoint;
            StartPoint = startPoint;
            int size = rand.Next(1, 30);
            MaxSize = new Size(size, size);
            if (delay == 0) { Delay = rand.Next(1, 100); } else { Delay = delay; }
            Closely = rand.Next(0, 10);
            Dir = new Point(Game.startPoint.X - Pos.X >= 0 ? rand.Next(1, 5) : -rand.Next(1, 5),
                Game.startPoint.Y - Pos.Y >= 0 ? rand.Next(1, 5) : -rand.Next(1, 5));
        }
    }
}
