using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Интерфейс для отрисовки объектов
    /// </summary>
    interface IDraw
    {
        /// <summary>
        /// Приоритет для отрисовки: 0 - отрисовывается последним, 1 - предпоследним и т.д.
        /// </summary>
        int DrawingPriority { get; }
        /// <summary>
        /// Близость траектории движения объекта к кораблю (0 - очень близко, 10 - очень далеко)
        /// </summary>
        int Closely { get; }
        /// <summary>
        /// Текущий размер объекта
        /// </summary>
        Size Size { get; }
        /// <summary>
        /// Отрисовывает объект на форме
        /// </summary>
        void Draw();
    }
}
