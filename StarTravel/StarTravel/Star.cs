using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class Star:BaseObject
    {
        public Star(Point pos, Point dir, Size size, int closely, int delay, Image image, Point? focusPoint = null, Size? maxSize = null, string text = "") 
            : base(pos, dir, size, closely, image, focusPoint, delay, maxSize, text)
        {
            
        }

        public override void Draw()
        {
            //Game.Buffer.Graphics.DrawLine(Pens.White, pos.X, pos.Y, pos.X + size.Width, pos.Y + size.Height);
            //Game.Buffer.Graphics.DrawLine(Pens.White, pos.X + size.Width, pos.Y, pos.X, pos.Y + size.Height);

            if (Delay != 0) { return; }
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, Size.Width, Size.Height);
            if (Text != string.Empty)
            {
                double offSet = 0.3;
                Font font = new Font("Verdana", (int)(Size.Width * offSet) >= 1 ? (int)(Size.Width * offSet) : 1);
                SolidBrush myBrush = new SolidBrush(Color.White);
                Game.Buffer.Graphics.DrawString(Text, font, myBrush, Pos.X, Pos.Y + (int)(Size.Height * (1 - offSet)));
            }
            

            //Game.Buffer.Graphics.ResetTransform();

            //using (GraphicsPath path = new GraphicsPath())
            //using (Brush brush = new TextureBrush(image))
            //using (Brush brush = new TextureBrush(image, new Rectangle(pos.X, pos.Y, 200, 200))) 
            //{
            //    path.AddPie(pos.X, pos.Y, size.Width, size.Height, 0, 360);
            //    Game.Buffer.Graphics.FillPath(brush, path);
            //}
        }

        public override void Update()
        {
            if (Delay != 0) { Delay--; return; }
            SpaceEngine.Update(this);
        }

        public override void NewStartPosition(int seedForRandom = 0, int delay = 0)
        {
            Pos = Game.startPoint;
            Size = new Size(1, 1);
        }

    }
}
