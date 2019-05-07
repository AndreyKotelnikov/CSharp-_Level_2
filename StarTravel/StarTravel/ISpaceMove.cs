using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    interface ISpaceMove
    {
        Point FocusPoint { get; }
        Point Pos { get; set; }
        Point Dir { get; }
        Size Size { get; set; }
        Size MaxSize { get; }
        int Closely { get; }

        void NewStartPosition(int seedForRandom, int delay);
    }
}
