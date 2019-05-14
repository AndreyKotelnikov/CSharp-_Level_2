using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Класс, который реализует метод для сравнения объектов отрисовки при сортировке
    /// </summary>
    class ComparisonForDrawing : IComparer<IDraw>
    {
        /// <summary>
        /// Метод для сравнения объектов отрисовки
        /// </summary>
        /// <param name="x">Первый объект для сравнения</param>
        /// <param name="y">Второй объект для сравнения</param>
        /// <returns>Возвращает результат сравнения: положительное или отрицательное число или ноль</returns>
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
