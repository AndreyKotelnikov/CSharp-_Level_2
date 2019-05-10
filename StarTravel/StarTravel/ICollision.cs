using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    interface ICollision
    {
        KindOfCollisionObject KindOfCollisionObject { get; }
        bool Collision(IDraw[] obj);
        Rectangle Rect { get; }
    }
}
