﻿using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    
    class Boom : BaseObject
    {
        private Image[] images;
        private int IndexImage;
        private int repeat;
        private int maxRepeat;
        internal bool EndBoom { get; private set; }

        public Boom(Point pos, Point dir, Size size, int closely, Image[] images, Point startPoint, int repeatEveryImage, 
            int drawingPriority, KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.NoDamageSpaceObject, 
            int delay = 0, Image image = null, Size? maxSize = null, string text = "")
            : base(pos, dir, size, closely,  delay, kindOfCollisionObject, image, startPoint, drawingPriority, maxSize, text)
        {
            this.images = images;
            IndexImage = 0;
            repeat = 0;
            maxRepeat = repeatEveryImage;
        }

        public override void Update()
        {
            SpaceEngine.Update(this);
        }

        public override void Draw()
        {
            int width = Size.Width + Size.Width * (IndexImage + 1) / 10;
            int height = Size.Height + Size.Height * (IndexImage + 1) / 10;

            Game.Buffer.Graphics.DrawImage(images[IndexImage], Pos.X - width/2, Pos.Y - height/2, 
                width , height);
            if (IndexImage < images.Length - 1)
            {
                repeat++;
                if (repeat >= maxRepeat)
                {
                    IndexImage++;
                    repeat = 0;
                }
            }
            else { EndBoom = true; }
            
        }
    }
}
