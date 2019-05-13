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
        private static Queue<LogEventArgs> logList;

        public Logger(bool writeToFile = false)
        {
            logList = new Queue<LogEventArgs>();
            WriteLogsToFile = writeToFile;
        }

        public int DrawingPriority => 0;

        public int Closely => 5;

        public Size Size => new Size();

        public bool WriteLogsToFile { get; set; }

        public void LoggingEvent(Object o, LogEventArgs logEvent)
        {
            if (logList.Count > 5)
            { logList.Dequeue(); }
            if (logEvent.SenderPropertyInfo?.Name == "IsBoom" && (logEvent.Sender as BaseObject)?.Closely == 0)
            {
                logEvent.Text = $"Убит опасный {logEvent.Sender.GetType().Name}";
            }
            if (WriteLogsToFile) { WriteToFile(logEvent, @"..\..\Логи.txt"); }
            logList.Enqueue(logEvent);
        }

        public void Draw()
        {
            int x = Game.Width - 200;
            int y = Game.Height - 100;

            Game.Buffer.Graphics.FillRectangle(Brushes.White, x, y, 200, 90);

            LogEventArgs[] arr = logList.ToArray();
            Array.Reverse(arr);
            Font font = new Font("Verdana", 10);
            Brush defaultBrush = Brushes.Black;
            Brush brush = defaultBrush;
            foreach (var item in arr)
            {
                brush = defaultBrush;
                if (item.SenderPropertyInfo?.Name == "IsBoom")
                {
                    if (item.Text != string.Empty) { brush = Brushes.Violet; }
                    else brush = Brushes.DarkBlue;
                }
                else if (item.SenderMethodInfo?.Name == "NewStartPosition" && item.ObjectToModifiedInMethod?.GetType().Name == "Ship")
                { brush = Brushes.Red; }

                
                Game.Buffer.Graphics.DrawString(item.Text == string.Empty ? item.ToString() : item.Text, font, brush, x, y);
                y += 15;
            }
        }

        private void WriteToFile(LogEventArgs logEvent, string path)
        {
            using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
            {
                sw.WriteLine($"{DateTime.Now}: {logEvent.ToString()} {logEvent.Sender.GetType().Name} {logEvent.SenderMethodInfo?.Name} " +
                    $"{logEvent.SenderPropertyInfo?.Name} {(logEvent.Sender as BaseObject)?.Closely} {logEvent.Text}");
            }
        }
    }
}
