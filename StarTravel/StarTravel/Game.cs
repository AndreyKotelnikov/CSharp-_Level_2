using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarTravel
{
    /// <summary>
    /// Запускает игру
    /// </summary>
    static class Game
    {
        private static Timer timer = new Timer { Interval = 30 };
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static Point StartPoint { get; private set; }
        public static Ship Ship { get; private set; }
        private static IDraw[] objsForGame;
        private static Image[] imageList;
        private static Image[] imageBoomList;
        private static Image imageBackGround;
        private static Image imageShip;
        private static Form form;
        private static int killAsteroids;
        private static ComparisonForDrawing comparisonForDrawing;

        public static event Action<int> KillAsteroid;

        /// <summary>
        /// Создаёт основные объекты игры
        /// </summary>
        public static void Load()
        {
            objsForGame = new IDraw[30];
            Random rand = new Random();
            int x = 1;
            int y = 1;
            int maxNumber = 10;
            for (int i = 0; i < objsForGame.Length - 2; i++)
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
                    objsForGame[i] = new Star(StartPoint, new Point(x, y), new Size(1, 1), rand.Next(0, 3), rand.Next(10) * 10, imageList[i]);
                    if ((objsForGame[i] as Star).ID == 2) { KillAsteroid += (objsForGame[i] as Star).Game_KillAsteroid; }
                }
                else if (i >= 3 && i <= 6)
                {
                    int XAst = (StartPoint.X + x * rand.Next(1, 100)) % Width;
                    int YAst = (StartPoint.Y + y * rand.Next(1, 100)) % Height;

                    objsForGame[i] = new Asteroid(new Point(XAst, YAst), new Point(StartPoint.X - XAst >= 0 ? 1 : -1,
                        StartPoint.Y - YAst >= 0 ? 1 : -1), new Size(1, 1), 2,
                        rand.Next(20, 50), imageList[i + 4], new Point(XAst, YAst), maxSize: new Size(20, 20));
                }
                else
                {
                    objsForGame[i] = new Star(StartPoint, new Point(x, y), new Size(1, 1), rand.Next(7, 11), rand.Next(10) * 10, imageList[rand.Next(3, 7)]);
                }

                objsForGame[objsForGame.Length - 2] = new Bullet(new Point(), new Point(), new Size(), 0, 0, imageShip, StartPoint);
                Ship = new Ship(imageShip, new Size(Width, Height));
                Ship.MessageDie += Ship_MessageDie;
                objsForGame[objsForGame.Length - 1] = Ship;

                //baseObjs[i] = new BaseObject(startPoint, new Point(-92, -1), new Size(1, 1), 0);
            }
            //for (int i = 0; i < _objs.Length; i++)
            //    _objs[i] = new BaseObject(new Point(600, i * 20), new Point(15 - i, 15 - i), new Size(20, 20), i.ToString());
            //for (int i = _objs.Length / 2; i < _objs.Length; i++)
            //    _objs[i] = new Star(new Point(600, (i - (_objs.Length / 2)) * 20), new Point(i, 0), new Size(20, 20), i.ToString());
            
            comparisonForDrawing = new ComparisonForDrawing();
        }

        private static void Ship_MessageDie(object obj, string message)
        {
            DialogResult result = MessageBox.Show("Вы отважно сражались! Вам понравилась игра?", "Game over", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show(message, "Game over"); 
            }
            else if (result == DialogResult.No)
            {
                MessageBox.Show("Нравится, то что получается - нужно больше тренироваться...", "Game over");
            }
            timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }

        /// <summary>
        /// Статический конструктор класса
        /// </summary>
        static Game()
        {
            killAsteroids = 0;
        }

        /// <summary>
        /// Производит инициализацию начальных настроек игры и загрузку картинок
        /// </summary>
        /// <param name="form">Ссылка на форму нужна для вывода сообщений об исключительных ситуациях</param>
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
            
            StartPoint = new Point(Width / 2, Height / 2);
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
            imageBackGround = Image.FromFile(@"..\..\Звёздное небо.png");
            imageShip = Image.FromFile(@"..\..\Корабль.png");
            imageBoomList = new Image[12];
            for (int i = 0; i < imageBoomList.Length; i++)
            {
                imageBoomList[i] = Image.FromFile($"..\\..\\Взрыв{i+1}.png");
            }

            Load();

            
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        /// <summary>
        /// Отрисовывает графические объекты игры
        /// </summary>
        public static void Draw()
        {
            // Проверяем вывод графики
            //Buffer.Graphics.Clear(Color.Black);
            //Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
            //Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
            //Buffer.Render();

            Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.DrawImage(imageBackGround, new Rectangle(0, 0, Width, Height));
            Array.Sort(objsForGame, comparisonForDrawing.Compare);
            foreach (IDraw obj in objsForGame)
                obj.Draw();
                        
            Buffer.Render();
        }

        /// <summary>
        /// Изменяет состояния объектов игры
        /// </summary>
        public static void Update()
        {
            try
            {
                foreach (IDraw obj in objsForGame)
                {
                    if (obj is IUpdate)
                    {
                        (obj as IUpdate).Update();
                        if (obj is ICollision 
                            && (obj as ICollision).KindOfCollisionObject == KindOfCollisionObject.DamageSpaceObject
                            && (!(obj is IBoom) || (obj as IBoom)?.IsBoom == false))
                        {
                            if ((obj as ICollision).Collision(objsForGame))
                            {
                                killAsteroids++;
                                KillAsteroid?.Invoke(killAsteroids);
                                System.Media.SystemSounds.Hand.Play();
                                (obj as IBoom)?.CreatBoom(imageBoomList, 2);
                            }
                        }
                    }
                }
            }
            catch (GameObjectException goe)
            {
                MessageBox.Show(goe.Message);
            }
            
                
        }

        /// <summary>
        /// Отрабатывает отрисовку и обновление объектов игры по событию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

    }

}
