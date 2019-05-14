using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Интерфейс для столкновения объектов
    /// </summary>
    interface ICollision
    {
        /// <summary>
        /// Указывает вид объекта из перечисления 
        /// </summary>
        KindOfCollisionObject KindOfCollisionObject { get; }
        /// <summary>
        /// Проверяет, есть ли столкновение текущего объекта с остальными
        /// </summary>
        /// <param name="obj">Массив объектов для проверки столкновения с ними</param>
        /// <returns>Возвращает true, если есть столкновение. Иначе false.</returns>
        bool Collision(IDraw[] obj);
        /// <summary>
        /// Область объекта на поле для проверки пересечения с областью другого объекта
        /// </summary>
        Rectangle Rect { get; }
    }
}
