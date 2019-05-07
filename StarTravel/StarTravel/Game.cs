using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarTravel
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static Point startPoint;
        public static BaseObject[] baseObjs;
        public static Image[] imageList;
        public static Image[] imageBoomList;
        public static Image backGround;
        public static Image ship;
        public static Form form;
        private static int killAsteroids;


        public static void Load()
        {
            baseObjs = new BaseObject[30];
            Random rand = new Random();
            int x = 1;
            int y = 1;
            int maxNumber = 10;
            for (int i = 0; i < baseObjs.Length - 1; i++)
            {
                switch (i % 4)
                {
                    case 0:
                        x = -rand.Next(1, maxNumber);
                        y = -rand.Next(1, maxNumber);
                        break;
                    case 1:
                        x = rand.Next(1, maxNumber);
                        y = -rand.Next(1, maxNumber);
                        break;
                    case 2:
                        x = -rand.Next(1, maxNumber);
                        y = rand.Next(1, maxNumber);
                        break;
                    case 3:
                        x = rand.Next(1, maxNumber);
                        y = rand.Next(1, maxNumber);
                        break;
                    default:
                        break;
                }
                if (Math.Abs(x) <= Math.Abs(y)) { x = x / Math.Abs(x); }
                else { y = y / Math.Abs(y); }
                if (i <= 2)
                {
                    baseObjs[i] = new Star(startPoint, new Point(x, y), new Size(1, 1), rand.Next(0, 3), rand.Next(10) * 10, imageList[i]);
                }
                else if (i >= 3 && i <= 6)
                {
                    int XAst = (startPoint.X + x * rand.Next(1, 100)) % Width;
                    int YAst = (startPoint.Y + y * rand.Next(1, 100)) % Height;

                    baseObjs[i] = new Asteroid(new Point(XAst, YAst), new Point(startPoint.X - XAst >= 0 ? 1 : -1,
                        startPoint.Y - YAst >= 0 ? 1 : -1), new Size(1, 1), 2,
                        rand.Next(20, 50), imageList[i + 4], new Point(XAst, YAst), new Size(40, 40));
                }
                else
                {
                    baseObjs[i] = new Star(startPoint, new Point(x, y), new Size(1, 1), rand.Next(7, 11), rand.Next(10) * 10, imageList[rand.Next(3, 7)]);
                }

                baseObjs[baseObjs.Length - 1] = new Bullet(new Point(), new Point(), new Size(), 0, 0, ship, startPoint);

                //baseObjs[i] = new BaseObject(startPoint, new Point(-92, -1), new Size(1, 1), 0);
            }
            //for (int i = 0; i < _objs.Length; i++)
            //    _objs[i] = new BaseObject(new Point(600, i * 20), new Point(15 - i, 15 - i), new Size(20, 20), i.ToString());
            //for (int i = _objs.Length / 2; i < _objs.Length; i++)
            //    _objs[i] = new Star(new Point(600, (i - (_objs.Length / 2)) * 20), new Point(i, 0), new Size(20, 20), i.ToString());

        }

        static Game()
        {
            killAsteroids = 0;
        }

        public static void Init(Form form)
        {
            Game.form = form;
            // Графическое устройство для вывода графики            
            Graphics g;
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            if (form.ClientSize.Width > 1000 || form.ClientSize.Width < 0 || form.ClientSize.Height > 1000 || form.ClientSize.Height < 0)
            {
                throw new Exception("Ups...", new ArgumentOutOfRangeException());
            }
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            
            startPoint = new Point(Width / 2, Height / 2);
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            imageList = new Image[11];
            imageList[0] = Image.FromFile(@"..\..\Звезда1.png");
            imageList[1] = Image.FromFile(@"..\..\Звезда2.png");
            imageList[2] = Image.FromFile(@"..\..\Звезда3.png");
            imageList[3] = Image.FromFile(@"..\..\Звезда_м1.png");
            imageList[4] = Image.FromFile(@"..\..\Звезда_м2.png");
            imageList[5] = Image.FromFile(@"..\..\Звезда_м3.png");
            imageList[6] = Image.FromFile(@"..\..\Звезда_м4.png");
            imageList[7] = Image.FromFile(@"..\..\Астероид1.png");
            imageList[8] = Image.FromFile(@"..\..\Астероид2.png");
            imageList[9] = Image.FromFile(@"..\..\Астероид3.png");
            imageList[10] = Image.FromFile(@"..\..\Астероид4.png");
            backGround = Image.FromFile(@"..\..\Звёздное небо.png");
            ship = Image.FromFile(@"..\..\Корабль.png");
            imageBoomList = new Image[12];
            for (int i = 0; i < imageBoomList.Length; i++)
            {
                imageBoomList[i] = Image.FromFile($"..\\..\\Взрыв{i+1}.png");
            }

            Load();

            Timer timer = new Timer { Interval = 30 };
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        public static void Draw()
        {
            // Проверяем вывод графики
            //Buffer.Graphics.Clear(Color.Black);
            //Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
            //Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
            //Buffer.Render();

            Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.DrawImage(backGround, new Rectangle(0, 0, Width, Height));
            Array.Sort(baseObjs);
            foreach (BaseObject obj in baseObjs)
                obj.Draw();
            
            Buffer.Graphics.DrawImage(ship, new Rectangle(0, 0, Width, Height));
            Buffer.Render();
        }

        public static void Update()
        {
            try
            {
                foreach (BaseObject obj in baseObjs)
                {
                    if (killAsteroids != 0 && obj.ID == 2)
                    {
                        obj.Text = $"Ты убил {killAsteroids} {Helper.InflectionOfWord(killAsteroids, "астероид", "астероида", "астероидов")}";
                    }
                    obj.Update();
                    if (obj is Asteroid && (obj as Asteroid).Boom == null)
                    {
                        if (obj.Collision(Bullet.BulletsList))
                        {
                            killAsteroids++;
                            System.Media.SystemSounds.Hand.Play();
                            (obj as Asteroid).CreatBoom(imageBoomList);
                            
                        }
                    }
                }
            }
            catch (GameObjectException goe)
            {
                MessageBox.Show(goe.Message);
            }
            
                
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

    }

}
