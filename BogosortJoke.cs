using System;

namespace Bogosort {
    //Сказать, что когда я прочитал первый раз задание, то я ничего не понял - это ничего не сказать(= К десятому прочтению смысл стал доходить до моего сознания
    //Возможно, всё дело в температуре)= самый мусорный алгоритм сортировки в мире... Так понимаю он тут исключительно, чтобы пошутить(=
    class Program {
        static void Main(string[] args) {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            int count = Int32.Parse(Console.ReadLine());
            string answer = String.Empty;
            while (count-- > 0) {
                string[] input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int[] array = new int[input.Length];
                for (var i = 0; i < array.Length; i++)
                    array[i] = Int32.Parse(input[i]);
                answer += $"{BoggortSort(ref array, 918255)} ";
            }
            Console.WriteLine(answer);
        }

        //шуточки про Боггортов в 2020(-=
        static int BoggortSort(ref int[] array, int seed = 918255) {
            int count = 0;
            long swapValue = seed;
            int[] temparr = new int[array.Length];
            for (var i = 0; i < array.Length; i++)
                temparr[i] = array[i];
            while (!CheckTruth(array)) {
                for(var i =0; i < array.Length; i++) {
                    NeumanRandomizer(ref swapValue);
                    int temp = array[i];
                    array[i] = array[swapValue % array.Length];
                    array[swapValue % array.Length] = temp;
                   
                    /*if (CheckTruth(array))
                        return count;*/
                    if (count> 7 && CompareArray(array, temparr))
                        return -1;
                }
                count++;
            }

            return count;
        }

        static bool CompareArray(int[] first, int[] second) {
            for(var i =0; i < first.Length; i++) {
                if (first[i] != second[i])
                    return false;
            }
            return true;
        }
            
        static bool CheckTruth(int[] array) {
            for (var i = 0; i < array.Length - 1; i++)
                if (array[i] > array[i + 1])
                    return false;
            return true;
        }
        static void NeumanRandomizer(ref long input) {
            long temp = input;
            long secondOne = temp / 1000;
            long secondTwo = temp % 1000*1000;
            long second = secondOne + secondTwo;
            temp = temp * second;
            temp /= 1000;
            temp %= 1000000;
            input = temp;

        }


 
    }

    
    
}
