using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class Logger : IDraw
    {
        /// <summary>
        /// Очередь для хранения логов в порядке их поступления
        /// </summary>
        private static Queue<LogEventArgs> logList;

        /// <summary>
        /// Конструктор экземпляра класса
        /// </summary>
        /// <param name="writeToFile">Требуется ли запись логов в файл?</param>
        public Logger(bool writeToFile = true, string filePath = @"..\..\Логи.txt")
        {
            logList = new Queue<LogEventArgs>();
            WriteLogsToFile = writeToFile;
            FilePath = filePath;
        }

        /// <summary>
        /// Приоритет для отрисовки: 0 - отрисовывается последним, 1 - предпоследним и т.д.
        /// </summary>
        public int DrawingPriority => 0;
        /// <summary>
        /// Близость траектории движения объекта к кораблю (0 - очень близко, 10 - очень далеко)
        /// </summary>
        public int Closely => 5;
        /// <summary>
        /// Текущий размер объекта
        /// </summary>
        public Size Size => new Size();

        /// <summary>
        /// Требуется ли запись логов в файл?
        /// </summary>
        public bool WriteLogsToFile { get; set; }
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Обрабатывает полученные события для логирования
        /// </summary>
        /// <param name="o">Отправитель данных для логирования</param>
        /// <param name="logEvent">Объект с данными о логируемом событии</param>
        public void LoggingEvent(Object o, LogEventArgs logEvent)
        {
            if (logList.Count > 5)
            { logList.Dequeue(); }
            if (logEvent.SenderPropertyInfo?.Name == "IsBoom" && (logEvent.Sender as BaseObject)?.Closely == 0)
            {
                logEvent.Text = $"Убит опасный {logEvent.Sender.GetType().Name}";
            }
            if (WriteLogsToFile) { WriteToFile(logEvent, FilePath); }
            logList.Enqueue(logEvent);
        }

        /// <summary>
        /// Отрисовывает объект на форме
        /// </summary>
        public void Draw()
        {
            int x = Game.Width - 300;
            int y = Game.Height - 100;

            if (logList.Count > 0)
            {
                Game.Buffer.Graphics.FillRectangle(Brushes.White, x, y, 280, 90);
            }

            LogEventArgs[] arr = logList.ToArray();
            Array.Reverse(arr);
            Font font = new Font("Verdana", 10);
            Brush defaultBrush = Brushes.Black;
            Brush brush = defaultBrush;
            foreach (var item in arr)
            {
                if (item.SenderPropertyInfo?.Name == "IsBoom")
                {
                    if (item.Sender.GetType().Name == "FirstAidKit") { brush = Brushes.Green; }
                    else if (item.Text != string.Empty) { brush = Brushes.Violet; }
                    else brush = Brushes.DarkBlue;
                }
                else if (item.SenderMethodInfo?.Name == "NewStartPosition" && item.ObjectToModifiedInMethod?.GetType().Name == "Ship")
                { brush = Brushes.Red; }

                Game.Buffer.Graphics.DrawString(item.Text == string.Empty ? item.ToString() : item.Text, font, brush, x, y);
                y += 15;
                brush = defaultBrush;
            }
        }
        /// <summary>
        /// Записывает данные в файл
        /// </summary>
        /// <param name="logEvent">Объект с данными о логируемом событии</param>
        /// <param name="filePath">Путь к файлу</param>
        private void WriteToFile(LogEventArgs logEvent, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default))
            {
                sw.WriteLine($"{DateTime.Now}: {logEvent.ToString()} {logEvent.Sender.GetType().Name} {logEvent.SenderMethodInfo?.Name} " +
                    $"{logEvent.SenderPropertyInfo?.Name} {(logEvent.Sender as BaseObject)?.Closely} {logEvent.Text}");
            }
        }
    }
}
