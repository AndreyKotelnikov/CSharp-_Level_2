using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Интерфейс для объектов, которые нужно перемещать в космическом пространстве
    /// </summary>
    interface ISpaceMove
    {
        /// <summary>
        /// Точка, из которой (или в которую) движется объект
        /// </summary>
        Point FocusPoint { get; }
        /// <summary>
        /// Текущая позиция объекта
        /// </summary>
        Point Pos { get; set; }
        /// <summary>
        /// Направление движения объекта
        /// </summary>
        Point Dir { get; }
        /// <summary>
        /// Текущий размер объекта
        /// </summary>
        Size Size { get; set; }
        /// <summary>
        /// Максимальный размер объекта
        /// </summary>
        Size MaxSize { get; }
        /// <summary>
        /// Близость траектории движения объекта к кораблю (0 - очень близко, 10 - очень далеко)
        /// </summary>
        int Closely { get; }
        /// <summary>
        /// Генерирует новую случайную позицию объекта 
        /// </summary>
        /// <param name="seedForRandom">Семено для класса Random, чтобы избежать слипания объектов, 
        /// которые генерируют новую позицию практически одновременно</param>
        /// <param name="delay">Задержка отрисовки объекта на определённое количество попыток его отрисовки</param>
        void NewStartPosition(int seedForRandom, int delay);
    }
}
