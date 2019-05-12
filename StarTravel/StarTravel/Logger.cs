using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class Logger : IDraw
    {
        private static Queue<string> logList;

        public Logger()
        {
            logList = new Queue<string>();
        }

        public int DrawingPriority => 0;

        public int Closely => 0;

        public Size Size => new Size();

        public void Game_Explode(string obj)
        {
            if (logList.Count > 5)
            { logList.Dequeue(); }
            logList.Enqueue(obj);
        }

        public void Draw()
        {
            int x = Game.Width - 200;
            int y = Game.Height - 100;

            Game.Buffer.Graphics.FillRectangle(Brushes.White, x, y, 150, 90);

            string[] arr = logList.ToArray();
            Array.Reverse(arr);
            foreach (var item in arr)
            {
                Font font = new Font("Verdana", 20);
                Game.Buffer.Graphics.DrawString(item, SystemFonts.DefaultFont, Brushes.Red, x, y);
                y += 15;
            }
        }
    }
}
