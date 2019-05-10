using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    public class Ship : IDraw
    {
        public Point Pos { get; private set; }
        public Size Size { get;  private set; }

        public int DrawingPriority { get; private set; }

        public int Closely { get; private set; }

        private Image image;
        
        public Ship(Image image, Size size, Point? pos = null, int drawingPriority = 0, int closely = 0)
        {
            Pos = pos?? new Point(0, 0);
            Size = size;
            this.image = image;
            DrawingPriority = drawingPriority;
            Closely = closely;
        } 

        public void Draw()
        {
            Game.Buffer.Graphics.DrawImage(image, new Rectangle(Pos, Size));
        }
    }
}
