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
        }

        public override void Update()
        {
            if (delay != 0) { delay--; return; }
            SpaceEngine.Update(this);
        }

        public override void Draw()
        {
            base.Draw();
            Game.Buffer.Graphics.DrawImage(image, pos.X, pos.Y, size.Width, size.Height);
            //Font font = new Font("Verdana", (int)(size.Width * 0.9) >= 1 ? (int)(size.Width * 0.9) : 1);
            //SolidBrush myBrush = new SolidBrush(Color.White);
            //Game.Buffer.Graphics.DrawString(text, font, myBrush, pos.X + 1, pos.Y + 1);
        }
    }
}
