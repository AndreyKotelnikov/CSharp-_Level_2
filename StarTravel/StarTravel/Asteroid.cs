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
        public Asteroid(Point pos, Point dir, Size size, int closely, int delay, Image image, 
            Point focusPoint, KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.DamageSpaceObject, 
            int drawingPriority = 5, Size? maxSize = null, string text = "")
            : base(pos, dir, size, closely, drawingPriority, kindOfCollisionObject, image, focusPoint, delay, maxSize, text)
        {
            FocusPoint = focusPoint;
            Boom = null;
            Text = Closely == 0 ? "Летит в корабль" : "";
        }
        
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

        public override void Draw()
        {
            if (Boom != null) { Boom.Draw(); }
            else
            {
                if (Delay != 0) { return; }
                Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, Size.Width, Size.Height);
                base.Draw();
            }
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
            Closely = rand.Next(0, 3);
            Text = Closely == 0 ? "Летит в корабль" : "";
            Dir = new Point(Game.ScreenCenterPoint.X - Pos.X >= 0 ? rand.Next(1, 5) : -rand.Next(1, 5),
                Game.ScreenCenterPoint.Y - Pos.Y >= 0 ? rand.Next(1, 5) : -rand.Next(1, 5));
        }
    }
}
