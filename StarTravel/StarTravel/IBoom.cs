using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    interface IBoom
    {
        bool IsBoom { get; }
        Boom Boom { get; }
        void CreatBoom(Image[] images, int timesOfRepeatEveryImage);
        
        event Action<string> Explode;
    }
}
