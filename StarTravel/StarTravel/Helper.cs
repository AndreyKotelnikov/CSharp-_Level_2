using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    static class Helper
    {
        public static string InflectionOfWord(int number, string wordFor1, string wordFor234, string wordFor056789)
        {
            if (number % 10 == 1 && number % 100 != 11) { return wordFor1; }
            else if (number % 10 >= 2 && number % 10 <= 4 && (number % 100 < 10 || number % 100 > 20)) { return wordFor234; }
            else { return wordFor056789; }
        }
    }
}
