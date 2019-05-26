using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Интерфейс для взрыва объекта
    /// </summary>
    interface IBoom
    {
        /// <summary>
        /// Объект взорвался?
        /// </summary>
        bool IsBoom { get; }
        /// <summary>
        /// Ссылка на объект, который отображает взрыв
        /// </summary>
        Boom Boom { get; }
        /// <summary>
        /// Взрывает текущий объект и создаёт объект, который отображает взрыв
        /// </summary>
        /// <param name="images">Массив картинок для отображения взрыва</param>
        /// <param name="timesOfRepeatEveryImage">Количество повторений отрисовки каждой картинки взрыва</param>
        void CreatBoom(Image[] images, int timesOfRepeatEveryImage);

        /// <summary>
        /// ID объекта
        /// </summary>
        int ID { get; }
    }
}
