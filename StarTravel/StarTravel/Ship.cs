using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarTravel
{
    /// <summary>
    /// Класс, описывающий Корабль
    /// </summary>
    class Ship : IDraw, ICollision
    {
        /// <summary>
        /// Текущая позиция объекта
        /// </summary>
        public Point Pos { get; private set; }
        /// <summary>
        /// Текущий размер объекта
        /// </summary>
        public Size Size { get;  private set; }
        /// <summary>
        /// Приоритет для отрисовки: 0 - отрисовывается последним, 1 - предпоследним и т.д.
        /// </summary>
        public int DrawingPriority { get; private set; }
        /// <summary>
        /// Близость траектории движения объекта к кораблю (0 - очень близко, 10 - очень далеко)
        /// </summary>
        public int Closely { get; private set; }
        /// <summary>
        /// Указывает вид объекта из перечисления
        /// </summary>
        public KindOfCollisionObject KindOfCollisionObject { get; private set; }

        /// <summary>
        /// Область объекта на поле для проверки пересечения с областью другого объекта
        /// </summary>
        public Rectangle Rect { get { return new Rectangle(Pos, Size); } }

        /// <summary>
        /// Высота кабины корабля, когда заканчивается приборная доска и виден уже космос в окно
        /// </summary>
        public int HeightOfCab { get; private set; }
        /// <summary>
        /// Картинка для отображения корабля
        /// </summary>
        private Image image;
        /// <summary>
        /// Максимальная энергия, которая может быть у корабля
        /// </summary>
        private int maxEnergy;
        /// <summary>
        /// Текущий уровень энергии у корабля
        /// </summary>
        private int energy;
        /// <summary>
        /// Уровень энергии у корабля
        /// </summary>
        public int Energy
        {
            get => energy;
            private set
            {
                if (value <= 0 && energy > 0) { energy = value; Die(); }
                energy = value;
            }
        }
        /// <summary>
        /// Понижает уровень энергии на указанную величину
        /// </summary>
        /// <param name="n">Величина , на которую понижается уровень энергии</param>
        public void EnergyLow(int n)
        {
            Energy -= n;
        }
        /// <summary>
        /// Повышает уровень энергии на указанную величину
        /// </summary>
        /// <param name="n">Величина , на которую повышается уровень энергии</param>
        public void EnergyUp(int n)
        {
            Energy = Energy + n > maxEnergy ? maxEnergy : Energy + n;
        }
        /// <summary>
        /// Делегат для хранения методов
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="message">Сообщение</param>
        public delegate void EventMessage(object obj, string message);
        /// <summary>
        /// Событие, которое активируется при смерти корабля
        /// </summary>
        public event EventMessage MessageDie;

        //public event EventMessage SubscribeToDie
        //{
        //    add { MessageDie += value; }
        //    remove { MessageDie -= value; }
        //}

        /// <summary>
        /// Конструктор экземпляра класса
        /// </summary>
        /// <param name="image"></param>
        /// <param name="size"></param>
        /// <param name="pos"></param>
        /// <param name="drawingPriority"></param>
        /// <param name="closely"></param>
        /// <param name="kindOfCollisionObject"></param>
        /// <param name="heightOfCab"></param>
        /// <param name="maxEnergy"></param>
        public Ship(Image image, Size size, Point? pos = null, int drawingPriority = 0, int closely = 10, 
            KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.Ship, int heightOfCab = 380, int maxEnergy = 100)
        {
            Pos = pos?? new Point(0, 0);
            Size = size;
            this.image = image;
            DrawingPriority = drawingPriority;
            Closely = closely;
            KindOfCollisionObject = kindOfCollisionObject;
            HeightOfCab = heightOfCab;
            this.maxEnergy = maxEnergy;
            energy = maxEnergy;
        } 
        /// <summary>
        /// отрисовывает объект на форме
        /// </summary>
        public void Draw()
        {
            Game.Buffer.Graphics.DrawImage(image, new Rectangle(Pos, Size));
            Game.Buffer.Graphics.DrawString($"Energy: {Energy}", SystemFonts.DefaultFont, Brushes.White, 0, 0);
        }
        /// <summary>
        /// Проверяет, есть ли столкновение текущего объекта с остальными 
        /// </summary>
        /// <param name="obj">Массив объектов для проверки столкновения с ними</param>
        /// <returns>Возвращает true, если есть столкновение. Иначе false.</returns>
        public bool Collision(IDraw[] obj)
        {
            foreach (var item in obj)
            {
                if (item is ICollision && (item as ICollision).KindOfCollisionObject == KindOfCollisionObject.DamageSpaceObject
                    && (item as ICollision).Rect.IntersectsWith(Rect)) { return true; }
            }
            return false;
        }
        /// <summary>
        /// Активирует событие о смерти Корабля
        /// </summary>
        private void Die()
        {
            MessageDie?.Invoke(this, "Кораблик умер...");
        }

    }
}
