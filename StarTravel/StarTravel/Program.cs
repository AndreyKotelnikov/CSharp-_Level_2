using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarTravel
{
    
    //Выполнил Андрей Котельников
    //ГБ Курс C# Уровень 2

    class Program
    {
        /// <summary>
        /// Начальная точка входа в программу
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Создаём форму
            Form form = new Form()
            {
                Width = 1000,
                Height = 600
            };
            //Запускаем инициализацию объектов игры
            Game.Init(form);
            //Отображаем форму
            form.Show();
            //Отрисовываем объекты игры на форме
            Game.Draw();
            //Передаём управление на форму
            Application.Run(form);
        }
    }

    
}
