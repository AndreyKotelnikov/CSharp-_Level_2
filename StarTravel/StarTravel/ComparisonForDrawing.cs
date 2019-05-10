using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class ComparisonForDrawing : IComparer<IDraw>
    {
        public int Compare(IDraw x, IDraw y)
        {
            if (x.DrawingPriority < y.DrawingPriority) { return 1; }
            if (x.DrawingPriority > y.DrawingPriority) { return -1; }
            if (x.Closely < y.Closely) { return 1; }
            else if (x.Closely > y.Closely) { return -1; }
            if (x.Size.Width > y.Size.Width) { return 1; }
            else if (x.Size.Width < y.Size.Width) { return -1; }
            return 0;
        }
    }
}
