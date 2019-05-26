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
        /// <summary>
        /// Таймер, определяет частоту отрисовки и изменения данных в игре
        /// </summary>
        private static Timer timer = new Timer { Interval = 30 };
        /// <summary>
        ///  Контекст, необходимый для создания буфера графики
        /// </summary>
        private static BufferedGraphicsContext _context;
        /// <summary>
        /// Буфер графики, где сначала отрисовываются все нужные объекты и потом выводяся на форму
        /// </summary>
        public static BufferedGraphics Buffer { get; private set; }
        /// <summary>
        /// Ширина игрового поля
        /// </summary>
        public static int Width { get; private set; }
        /// <summary>
        /// Высота игрового поля
        /// </summary>
        public static int Height { get; private set; }
        /// <summary>
        /// Координаты центра игрового поля
        /// </summary>
        public static Point ScreenCenterPoint { get; private set; }
        /// <summary>
        /// Ссылка на объект Корабль для доступа из других классов
        /// </summary>
        public static Ship Ship { get; private set; }
        /// <summary>
        /// Массив объектов для отрисовки на форме
        /// </summary>
        private static IDraw[] objsForGame;
        /// <summary>
        /// Массив картинок, которые используются для отображения космических объектов
        /// </summary>
        private static Image[] imageList;
        /// <summary>
        /// Массив картинок, которые иммитируют динамическое отображение взрыва при последовательном показе
        /// </summary>
        private static Image[] imageBoomList;
        /// <summary>
        /// Картинка для отображения на заднем фоне
        /// </summary>
        private static Image imageBackGround;
        /// <summary>
        /// Картинка для отображения кабины корабля
        /// </summary>
        private static Image imageShip;
        /// <summary>
        /// Ссылка на форму для вывода дополнительных сообщений в отдельных диалоговых окнах
        /// </summary>
        private static Form form;
        /// <summary>
        /// Количество убитых астероидов
        /// </summary>
        private static int killAsteroids;
        /// <summary>
        /// Ссылка на класс, который выполняет логирование
        /// </summary>
        private static Logger logger;
        /// <summary>
        /// Ссылка на класс, который реализует метод для сравнения объектов отрисовки при сортировке
        /// </summary>
        private static ComparisonForDrawing comparisonForDrawing;
        /// <summary>
        /// Событие, которое активируется при изменении количества убитых астероидов
        /// </summary>
        public static event Action<int> KillAsteroid;

        private static List<IDraw> asteroidsGroup;

        private static void CreateAsteroidsGroup(int length)
        {
            Random rand = new Random();
            int x;
            int y;

            for (int i = 0; i < length; i++)
            {
                GetXY(i, 10, out x, out y);

                int XAst = x > 0 ? rand.Next(1, (int)(Width * 0.1)) : rand.Next((int)(Width * 0.9), Width);
                int YAst = x > 0 ? rand.Next(1, (int)(Height * 0.1)) : rand.Next((int)(Height * 0.9), Height);

                
                asteroidsGroup.Add(new Asteroid(new Point(XAst, YAst), new Point(ScreenCenterPoint.X - XAst >= 0 ? 1 : -1,
                ScreenCenterPoint.Y - YAst >= 0 ? 1 : -1), new Size(1, 1), 2,
                rand.Next(20, 50), imageList[i + 4], new Point(XAst, YAst), maxSize: new Size(20, 20)));
                
            }
        }

        private static void GetXY(int i, int maxNumber, out int x, out int y)
        {
            Random rand = new Random();
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
                    x = 1;
                    y = 1;
                    break;
            }
        }

        /// <summary>
        /// Создаёт основные объекты игры и делает необходимые подписки на события
        /// </summary>
        public static void Load()
        {
            logger = new Logger();
            objsForGame = new IDraw[30];
            Random rand = new Random();
            int x;
            int y;
            for (int i = 0; i < objsForGame.Length - 5; i++)
            {
                GetXY(i, 10, out x, out y);
                
                if (Math.Abs(x) <= Math.Abs(y)) { x = x / Math.Abs(x); }
                else { y = y / Math.Abs(y); }
                if (i <= 2)
                {
                    objsForGame[i] = new Star(ScreenCenterPoint, new Point(x, y), new Size(1, 1), rand.Next(0, 3), rand.Next(10) * 10, imageList[i]);
                    if ((objsForGame[i] as Star).ID == 1) { KillAsteroid += (objsForGame[i] as Star).Game_KillAsteroid; }
                }
                else if (i >= 3 && i <= 7)
                {
                    int XAst = (ScreenCenterPoint.X + x * rand.Next(1, 100)) % Width;
                    int YAst = (ScreenCenterPoint.Y + y * rand.Next(1, 100)) % Height;

                    if (i == 7)
                    {
                        objsForGame[i] = new FirstAidKit(new Point(XAst, YAst), new Point(ScreenCenterPoint.X - XAst >= 0 ? 1 : -1,
                        ScreenCenterPoint.Y - YAst >= 0 ? 1 : -1), new Size(1, 1), 2,
                        rand.Next(20, 50), imageList[i + 4], new Point(XAst, YAst), maxSize: new Size(10, 10));
                    }
                    else
                    {
                        objsForGame[i] = new Asteroid(new Point(XAst, YAst), new Point(ScreenCenterPoint.X - XAst >= 0 ? 1 : -1,
                        ScreenCenterPoint.Y - YAst >= 0 ? 1 : -1), new Size(1, 1), 2,
                        rand.Next(20, 50), imageList[i + 4], new Point(XAst, YAst), maxSize: new Size(20, 20));
                    }
                }
                else
                {
                    objsForGame[i] = new Star(ScreenCenterPoint, new Point(x, y), new Size(1, 1), rand.Next(7, 11), rand.Next(10) * 10, imageList[rand.Next(3, 7)]);
                }
                if (objsForGame[i] is ILog) { (objsForGame[i] as ILog).Logging += logger.LoggingEvent; }
            }
            Ship = new Ship(imageShip, new Size(Width, Height));
            Ship.MessageDie += Ship_MessageDie;
            objsForGame[objsForGame.Length - 1] = Ship;
            objsForGame[objsForGame.Length - 2] = new Bullet(new Point(), new Point(), new Size(), 0, 0, imageShip, ScreenCenterPoint);
            form.KeyDown += (objsForGame[objsForGame.Length - 2] as Bullet).Form_KeyDown;
            form.MouseClick += (objsForGame[objsForGame.Length - 2] as Bullet).Form_MouseClick;
            objsForGame[objsForGame.Length - 3] = new FrontSight();
            form.KeyDown += (objsForGame[objsForGame.Length - 3] as FrontSight).Form_KeyDown;
            (objsForGame[objsForGame.Length - 3] as FrontSight).ChangeFocusPoint += (objsForGame[objsForGame.Length - 2] as Bullet).FrontSight_ChangeFocusPoint;
            form.MouseMove += (objsForGame[objsForGame.Length - 3] as FrontSight).Form_MouseMove;
            objsForGame[objsForGame.Length - 4] = logger;
            objsForGame[objsForGame.Length - 5] = logger;

            comparisonForDrawing = new ComparisonForDrawing();
        }

        /// <summary>
        /// Метод запускается при смерти корабля
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        private static void Ship_MessageDie(object obj, string message)
        {
            // !!! Выходит ошибка - this равно null - пока не знаю почему.
            //form.KeyDown -= (objsForGame[objsForGame.Length - 2] as Bullet).Form_KeyDown;
            //form.KeyDown -= (objsForGame[objsForGame.Length - 3] as FrontSight).Form_KeyDown;
            //form.MouseClick -= (objsForGame[objsForGame.Length - 2] as Bullet).Form_MouseClick;
            //form.MouseMove -= (objsForGame[objsForGame.Length - 3] as FrontSight).Form_MouseMove;

            Cursor.Show();

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
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), 
                Brushes.Yellow, ScreenCenterPoint.X - 150, ScreenCenterPoint.Y - 130);
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
        /// <param name="form">Ссылка на форму нужна для вывода сообщений об исключительных ситуациях и диалогов</param>
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
            //Прячем курсор мыши
            Cursor.Hide();
            //Расчитываем координаты центра игрового поля
            ScreenCenterPoint = new Point(Width / 2, Height / 2);
            
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            //Загружаем картинки
            imageList = new Image[12];
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
            imageList[11] = Image.FromFile(@"..\..\Аптечка.png");
            imageBackGround = Image.FromFile(@"..\..\Звёздное небо.png");
            imageShip = Image.FromFile(@"..\..\Корабль.png");
            imageBoomList = new Image[12];
            for (int i = 0; i < imageBoomList.Length; i++)
            {
                imageBoomList[i] = Image.FromFile($"..\\..\\Взрыв{i+1}.png");
            }
            //Запускаем создание объектов игры
            Load();
            //Запускаем таймер и делаем подписку на событие
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        /// <summary>
        /// Отрисовывает графические объекты игры
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.DrawImage(imageBackGround, new Rectangle(0, 0, Width, Height));
            Array.Sort(objsForGame, comparisonForDrawing.Compare);
            foreach (IDraw obj in objsForGame)
            {
                obj.Draw();
            }
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
                        //Проверяем: может ли объект сталкиваться и взрываться.
                        if (obj is ICollision && (!(obj is IBoom) || (obj as IBoom)?.IsBoom == false))
                        {
                            //Проверяем разрушаемые объекты на столкновение с оружием
                            if ((obj as ICollision).KindOfCollisionObject == KindOfCollisionObject.DamageSpaceObject)
                            {
                                if ((obj as ICollision).Collision(objsForGame))
                                {
                                    killAsteroids++;
                                    KillAsteroid?.Invoke(killAsteroids);
                                    System.Media.SystemSounds.Hand.Play();
                                    (obj as IBoom)?.CreatBoom(imageBoomList, 2);
                                }
                            }
                            //Проверяем лечащие объекты на столкновение с оружием
                            else if ((obj as ICollision).KindOfCollisionObject == KindOfCollisionObject.HealingSpaceObject)
                            {
                                if ((obj as ICollision).Collision(objsForGame))
                                {
                                    System.Media.SystemSounds.Beep.Play();
                                    (obj as IBoom)?.CreatBoom(imageBoomList, 2);
                                }
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
