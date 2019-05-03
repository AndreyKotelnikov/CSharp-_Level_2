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
        protected Image image;

        public Star(Point pos, Point dir, Size size, int closely, int delay, Image image, Size? maxSize = null, string number = "") 
            : base(pos, dir, size, closely, delay, maxSize, number)
        {
            this.image = image;
        }

        public override void Draw()
        {
            //Game.Buffer.Graphics.DrawLine(Pens.White, pos.X, pos.Y, pos.X + size.Width, pos.Y + size.Height);
            //Game.Buffer.Graphics.DrawLine(Pens.White, pos.X + size.Width, pos.Y, pos.X, pos.Y + size.Height);

            Game.Buffer.Graphics.DrawImage(image, pos.X, pos.Y, size.Width, size.Height);
            //Game.Buffer.Graphics.ResetTransform();
            
            //using (GraphicsPath path = new GraphicsPath())
            //using (Brush brush = new TextureBrush(image))
            //using (Brush brush = new TextureBrush(image, new Rectangle(pos.X, pos.Y, 200, 200))) 
            //{
            //    path.AddPie(pos.X, pos.Y, size.Width, size.Height, 0, 360);
            //    Game.Buffer.Graphics.FillPath(brush, path);
            //}
        }

        

    }
}
