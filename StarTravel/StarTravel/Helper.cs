using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Вспомогательный класс для реализации дополнительных методов обработки данных
    /// </summary>
    static class Helper
    {
        /// <summary>
        /// Склонение слова от числа
        /// </summary>
        /// <param name="number">Число, по которому определяется склонение слова</param>
        /// <param name="wordFor1">Слово для числа 1</param>
        /// <param name="wordFor234">Слово для чисел 2, 3, 4</param>
        /// <param name="wordFor056789">Слово для чисел 0, 5-9</param>
        /// <returns>Возвращает нужное склонение слова</returns>
        public static string InflectionOfWord(int number, string wordFor1, string wordFor234, string wordFor056789)
        {
            if (number % 10 == 1 && number % 100 != 11) { return wordFor1; }
            else if (number % 10 >= 2 && number % 10 <= 4 && (number % 100 < 10 || number % 100 > 20)) { return wordFor234; }
            else { return wordFor056789; }
        }
    }
}
