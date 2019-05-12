using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarTravel
{
    class Bullet : BaseObject
    {
        private static Point LeftGun { get; set; }
        private static Point RightGun { get; set; }
        public override Rectangle Rect { get { return new Rectangle(FocusPoint, Size); } }
        public int TimeOfShooting { get; private set; }
        private readonly int maxTimeOfShooting;



        static Bullet()
        {
            int heightOfGuns = 380;
            
            LeftGun = new Point(213, heightOfGuns + 7);
            RightGun = new Point(794, heightOfGuns);
        }

        public Bullet(Point pos, Point dir, Size size, int closely, int delay, Image image, Point focusPoint, 
            KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.Weapon, int drawingPriority = 1, 
            Size? maxSize = null, string text = "", int maxTimeOfShooting = 5)
            : base(pos, dir, size, closely, drawingPriority, kindOfCollisionObject, image, focusPoint, delay, maxSize, text)
        {
            this.maxTimeOfShooting = maxTimeOfShooting;
            TimeOfShooting = 0;
        }

        public override void Update()
        {
            if (Delay != 0) { Delay--; return; }
           
            
        }

        public override void Draw()
        {
            if (TimeOfShooting <= 0) { return; }
            TimeOfShooting--;
            if (Delay != 0) { return; }
            //Game.Buffer.Graphics.DrawImage(image, pos.X, pos.Y, size.Width, size.Height);
            Pen pen = new Pen(Color.Red, 4);
            Game.Buffer.Graphics.DrawLine(pen, LeftGun, FocusPoint);
            Game.Buffer.Graphics.DrawLine(pen, RightGun, FocusPoint);
            Delay = 2;
        }

        public override bool Collision(IDraw[] obj)
        {
            foreach (var item in obj)
            {
                if (item is ICollision && (item as ICollision).KindOfCollisionObject == KindOfCollisionObject.DamageSpaceObject
                    && (item as ICollision).Rect.IntersectsWith(Rect)) { return true; }
            }
            return false;
        }

        public void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                TimeOfShooting = maxTimeOfShooting;
            }
        }

        public void FrontSight_ChangeFocusPoint(Point obj)
        {
            FocusPoint = obj;
        }

        internal void Form_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TimeOfShooting = maxTimeOfShooting;
            }
        }
    }
}
