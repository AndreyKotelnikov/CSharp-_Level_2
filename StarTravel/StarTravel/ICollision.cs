using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    interface ICollision
    {
        bool Collision(List<Bullet> obj);
        Rectangle Rect { get; }
    }
}
