using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class Bullet : BaseObject
    {
        internal Point FocusPoint { get; private set; }
        internal static List<Bullet> BulletsList { get; set; }

        static Bullet()
        {
            BulletsList = new List<Bullet>();
        }

        public Bullet(Point pos, Point dir, Size size, int closely, int delay, Image image, Point focusPoint, Size? maxSize = null, string text = "")
            : base(pos, dir, size, closely, image, delay, maxSize, text)
        {
            BulletsList.Add(this);
            FocusPoint = focusPoint;
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
        }
    }
}
