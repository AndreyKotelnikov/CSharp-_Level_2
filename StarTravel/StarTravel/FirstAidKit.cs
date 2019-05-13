using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class FirstAidKit : Asteroid
    {
        public override bool IsBoom
        {
            get => isBoom;
            protected set
            {
                if (isBoom == false && value == true)
                {
                    Random rand = new Random();
                    int cure = rand.Next(MaxSize.Width, MaxSize.Width * 2);
                    Game.Ship.EnergyUp(cure);
                    LogEventArgs e = new LogEventArgs(this, null, GetType().GetProperty("IsBoom"), Game.Ship, cure, "вылечила жизни на");
                    StartLogginEvent(this, e);
                }
                isBoom = value;
            }
        }

        public FirstAidKit(Point pos, Point dir, Size size, int closely, int delay, Image image,
            Point focusPoint, KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.DamageSpaceObject,
            int drawingPriority = 5, Size? maxSize = null, string text = "") : base(pos, dir, size, closely, delay, image,
             focusPoint, kindOfCollisionObject, drawingPriority, maxSize, text)
        {
            Text = string.Empty;
        }

        public override void Update()
        {
            if (IsBoom)
            {
                NewStartPosition(seedForRandom, 100);
                seedForRandom++;
            }
            else
            {
                if (Delay != 0) { Delay--; return; }
                SpaceEngine.Update(this);
            }
        }

        public override void Draw()
        {
            if(Boom != null) { Boom = null; }
            base.Draw();
            Pen pen = new Pen(Color.Green, 2);
            Game.Buffer.Graphics.DrawEllipse(pen, Rect);
        }

        public override void NewStartPosition(int seedForRandom = 0, int delay = 0)
        {
            base.NewStartPosition();
            Random rand = new Random();
            Text = string.Empty;
            Delay = rand.Next(200, 400);
            int size = rand.Next(1, 10);
            MaxSize = new Size(size, size);
            if(Closely == 0) { Closely = rand.Next(1, 11); }
        }
    }
}
