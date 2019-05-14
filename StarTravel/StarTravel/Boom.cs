using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    
    class Boom : BaseObject
    {
        /// <summary>
        /// Массив картинок, которые иммитируют динамическое отображение взрыва при последовательном показе
        /// </summary>
        private Image[] images;
        /// <summary>
        /// Текущий индекс картинки из массива картинок для отображения взрыва
        /// </summary>
        private int IndexImage;
        /// <summary>
        /// Текущее повторение картинки при отображении на форме
        /// </summary>
        private int repeat;
        /// <summary>
        /// Максимальное количество повторений картинки при отображении на форме
        /// </summary>
        private int maxRepeat;
        /// <summary>
        /// Взрыв завершился?
        /// </summary>
        internal bool EndBoom { get; private set; }

        /// <summary>
        /// Конструктор для экземпляра класса
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="size"></param>
        /// <param name="closely"></param>
        /// <param name="images"></param>
        /// <param name="startPoint"></param>
        /// <param name="repeatEveryImage"></param>
        /// <param name="drawingPriority"></param>
        /// <param name="kindOfCollisionObject"></param>
        /// <param name="delay"></param>
        /// <param name="image"></param>
        /// <param name="maxSize"></param>
        /// <param name="text"></param>
        public Boom(Point pos, Point dir, Size size, int closely, Image[] images, Point startPoint, int repeatEveryImage, 
            int drawingPriority, KindOfCollisionObject kindOfCollisionObject = KindOfCollisionObject.NoDamageSpaceObject, 
            int delay = 0, Image image = null, Size? maxSize = null, string text = "")
            : base(pos, dir, size, closely,  delay, kindOfCollisionObject, image, startPoint, drawingPriority, maxSize, text)
        {
            this.images = images;
            IndexImage = 0;
            repeat = 0;
            maxRepeat = repeatEveryImage;
        }
        /// <summary>
        /// Переопределённый метод обновления данных объекта
        /// </summary>
        public override void Update()
        {
            SpaceEngine.Update(this);
        }
        /// <summary>
        /// Переопределённый метод отрисовки объекта на форме
        /// </summary>
        public override void Draw()
        {
            int width = Size.Width + Size.Width * (IndexImage + 1) / 10;
            int height = Size.Height + Size.Height * (IndexImage + 1) / 10;

            Game.Buffer.Graphics.DrawImage(images[IndexImage], Pos.X - width/2, Pos.Y - height/2, 
                width , height);
            if (IndexImage < images.Length - 1)
            {
                repeat++;
                if (repeat >= maxRepeat)
                {
                    IndexImage++;
                    repeat = 0;
                }
            }
            else { EndBoom = true; }
            
        }
    }
}
