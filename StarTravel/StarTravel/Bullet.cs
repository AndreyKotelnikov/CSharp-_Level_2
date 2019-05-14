using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarTravel
{
    class Bullet : BaseObject
    {
        /// <summary>
        /// Координаты левой пушки
        /// </summary>
        private static Point LeftGun { get; set; }
        /// <summary>
        /// Координаты правой пушки
        /// </summary>
        private static Point RightGun { get; set; }
        /// <summary>
        /// Переопределённое свойство, указывающее площадь объекта на форме
        /// </summary>
        public override Rectangle Rect { get { return new Rectangle(new Point(FocusPoint.X - Size.Width/2, FocusPoint.Y - Size.Height/2), Size); } }
        /// <summary>
        /// Длительность одного выстрела
        /// </summary>
        public int TimeOfShooting { get; private set; }
        /// <summary>
        /// Максимальная длительность одного выстрела
        /// </summary>
        private readonly int maxTimeOfShooting;


        /// <summary>
        /// Статический конструктор для статических свойств класса
        /// </summary>
        static Bullet()
        {
            //Высота орудий
            int heightOfGuns = 380;
            
            LeftGun = new Point(213, heightOfGuns + 7);
            RightGun = new Point(794, heightOfGuns);
        }

        /// <summary>
        /// Конструктор для экземпляра класса
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="size"></param>
        /// <param name="closely"></param>
        /// <param name="delay"></param>
        /// <param name="image"></param>
        /// <param name="focusPoint"></param>
        /// <param name="kindOfCollisionObject"></param>
        /// <param name="drawingPriority"></param>
        /// <param name="maxSize"></param>
        /// <param name="text"></param>
        /// <param name="maxTimeOfShooting"></param>
        public Bullet(Point pos, Point dir, Size size, int closely, int delay, Image image, Point focusPoint, 
            KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.Weapon, int drawingPriority = 1, 
            Size? maxSize = null, string text = "", int maxTimeOfShooting = 5)
            : base(pos, dir, size, closely, drawingPriority, kindOfCollisionObject, image, focusPoint, delay, maxSize, text)
        {
            this.maxTimeOfShooting = maxTimeOfShooting;
            TimeOfShooting = 0;
        }

        /// <summary>
        /// Переопределённый метод обновления данных объекта
        /// </summary>
        public override void Update()
        {
            if (Delay != 0) { Delay--; return; }
        }

        /// <summary>
        /// Переопределённый метод отрисовки объекта на форме
        /// </summary>
        public override void Draw()
        {
            if (TimeOfShooting <= 0) { return; }
            TimeOfShooting--;
            if (Delay != 0) { return; }
            //Game.Buffer.Graphics.DrawImage(image, pos.X, pos.Y, size.Width, size.Height);
            Pen pen = new Pen(Color.Red, 4);
            Game.Buffer.Graphics.DrawLine(pen, LeftGun, FocusPoint);
            Game.Buffer.Graphics.DrawLine(pen, RightGun, FocusPoint);
            Delay = 2;
        }

        /// <summary>
        /// Проверяет, есть ли столкновение текущего объекта с остальными
        /// </summary>
        /// <param name="obj">Массив объектов для проверки столкновения с ними</param>
        /// <returns>Возвращает true, если есть столкновение. Иначе false.</returns>
        public override bool Collision(IDraw[] obj)
        {
            foreach (var item in obj)
            {
                if (item is ICollision && (item as ICollision).KindOfCollisionObject == KindOfCollisionObject.DamageSpaceObject
                    && (item as ICollision).Rect.IntersectsWith(Rect)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Обработчик события при нажатии клавишы на клавиатуре
        /// </summary>
        /// <param name="sender">Отправитель события</param>
        /// <param name="e">Данные о нажатой клавише</param>
        public void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                TimeOfShooting = maxTimeOfShooting;
            }
        }

        /// <summary>
        /// Обработчик события при изменения фокуса прицела
        /// </summary>
        /// <param name="obj">Новая позиция прицела</param>
        public void FrontSight_ChangeFocusPoint(Point obj)
        {
            FocusPoint = obj;
        }

        /// <summary>
        /// Обработчик события о нажатии кнопки мыши
        /// </summary>
        /// <param name="sender">Отправитель события</param>
        /// <param name="e">Данные о событии мыши</param>
        internal void Form_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TimeOfShooting = maxTimeOfShooting;
            }
        }
    }
}
