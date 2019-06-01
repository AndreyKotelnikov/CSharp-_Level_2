using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;


namespace TrainingLinq
{
    public static class MyExtensions
    {
        public static Dictionary<int, int> CountInt(this List<int> listInt)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            foreach (var item in listInt)
            {
                if (dict.ContainsKey(item))
                {
                    dict[item]++;
                }
                else
                {
                    dict.Add(item, 1);
                }
            }
            return dict;
        }

        public static Dictionary<T, int> CountItems<T>(this List<T> listT)
        {
            Dictionary<T, int> dict = new Dictionary<T, int>();
            foreach (var item in listT)
            {
                if (dict.ContainsKey(item))
                {
                    dict[item]++;
                }
                else
                {
                    dict.Add(item, 1);
                }
            }
            return dict;
        }

        public static void Print<T>(this Dictionary<T, int> dict)
        {
            foreach (var item in dict)
            {
                Write($" {item}");
            }
        }

        public static void Print<T>(this List<T> list)
        {
            foreach (var item in list)
            {
                Write($" {item.ToString()}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //2.Дана коллекция List<T>. Требуется подсчитать, сколько раз каждый элемент встречается в данной коллекции:
            //a.для целых чисел;
            List<int> listInt = new List<int>()
            {
              1, 2, 3, 4, 2, 1, 1
            };
            foreach (var item in listInt)
            {
                Write($" {item}");
            }
            WriteLine("\n");
            listInt.CountInt().Print();

            //b.  * для обобщенной коллекции;
            WriteLine("\n\n");
            List<string> listStr = new List<string>()
            {
              "s1", "s2","s3","s2","s4","s1","s1","s1",
            };

            listStr.Print();
            WriteLine("\n");
            listStr.CountItems().Print();

            var countItems = from l in listStr
                             group l by l into g
                             select new { Name = g.Key, Count = g.Count() };


            WriteLine("\n\nLinq");
            foreach (var item in countItems)
            {
                Write($" {item.Name} = {item.Count},");
            }

            var countItems2 = listStr.GroupBy(l => l)
                .Select(s => new { s.Key, Count = s.Count() });

            WriteLine("\n\nLinq 2");
            foreach (var item in countItems2)
            {
                Write($" {item.Key} = {item.Count},");
            }

            var countItems3 = listStr.GroupBy(l => l);
                

            WriteLine("\n\nLinq 3");
            foreach (var item in countItems3)
            {
                Write($" {item.Key} = {item.Count()},");
            }


            ReadKey();
        }


        
    }
}
