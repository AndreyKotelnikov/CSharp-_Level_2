using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarTravel
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form()
            {
                Width = 1000,
                Height = 600
            };
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }

    
}
