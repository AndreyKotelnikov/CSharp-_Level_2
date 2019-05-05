using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class GameObjectException  : Exception
    {
        public GameObjectException(string massage) : base(massage)
        {

        }
    }
}
