using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Отдельный класс для исключений в игре
    /// </summary>
    class GameObjectException  : Exception
    {
        /// <summary>
        /// Конструктор для экземпляра класса
        /// </summary>
        /// <param name="massage">Текстовое сообщение</param>
        public GameObjectException(string massage) : base(massage)
        {

        }
    }
}
