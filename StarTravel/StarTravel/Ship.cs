using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class Ship : IDraw, ICollision
    {
        public Point Pos { get; private set; }
        public Size Size { get;  private set; }

        public int DrawingPriority { get; private set; }

        public int Closely { get; private set; }

        public KindOfCollisionObject KindOfCollisionObject { get; private set; }

        public Rectangle Rect { get { return new Rectangle(Pos, Size); } }

        private Image image;

        private int energy = 100;

        public int Energy => energy;

        public void EnergyLow(int n)
        {
            energy -= n;
        }


        public Ship(Image image, Size size, Point? pos = null, int drawingPriority = 0, int closely = 0, 
            KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.Ship)
        {
            Pos = pos?? new Point(0, 0);
            Size = size;
            this.image = image;
            DrawingPriority = drawingPriority;
            Closely = closely;
            KindOfCollisionObject = kindOfCollisionObject;
        } 

        public void Draw()
        {
            Game.Buffer.Graphics.DrawImage(image, new Rectangle(Pos, Size));
            Game.Buffer.Graphics.DrawString($"Energy: {Energy}", SystemFonts.DefaultFont, Brushes.White, 0, 0);
        }

        public bool Collision(IDraw[] obj)
        {
            foreach (var item in obj)
            {
                if (item is ICollision && (item as ICollision).KindOfCollisionObject == KindOfCollisionObject.DamageSpaceObject
                    && (item as ICollision).Rect.IntersectsWith(Rect)) { return true; }
            }
            return false;
        }

        public void Die()
        {
        }

    }
}
