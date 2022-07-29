using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;
namespace CodeAbbey {
   //Да-да, старая добрая комбинаторика и спустя десять лет продолжает баловать меня
    class Program {


        /*в принципе - это не лучшее из решений, но в конкретной ситуациия был плавленный, времени не было. Пришлось выбирать или делать или не делать(=
         * 
         */
        static void Main() {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string input = Console.ReadLine();
            List<string> answer = GetCombinationWithRep(input);
            foreach (var member in answer)
                Console.Write($"{member} ");
            /*List<int[]> Eight = GenerationCombination(8, 3);
            foreach (var member in Eight)
                Print(member);*/
                
            
        }

        static List<string> GetCombinationWithRep(string input) {
            string[] temp = input.Split(" of ");
            int m = Int32.Parse(temp[0]);
            temp = temp[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Dictionary<int, char> booking = new Dictionary<int, char>();
            for (var i = 0; i < temp.Length; i++)
                booking.Add(i, temp[i][0]);
            List<int[]> combos = GenerationCombination(booking.Count, m);
            List<string> answer = new List<string>();

            for (var i = 0; i < combos.Count; i++) {
                answer.Add(String.Empty);
                for (var j = 0; j < combos[i].Length; j++)
                    answer[i] += booking[combos[i][j]];
            }

            answer.Sort();
            for(var i =0; i < answer.Count -1; i++) {
                if (answer[i] == answer[i + 1]) {
                    answer.RemoveAt(i + 1);
                    i--;
                }
            }
            return answer;
        }
        static void Print(int[] x) {
            Console.Write("(");
            foreach (var member in x)
                Console.Write($"{member} ");
            Console.Write(")");
        }
        static int[] CloneArr(int[] x) {
            int[] clone = new int[x.Length];
            for (var i = 0; i < x.Length; i++)
                clone[i] = x[i];
            return clone;
        }
        /* Нужно найти сочетание по номеру, сначала думал построить решение на делении по модулю, но что-то как то на бумаге всё выглядело плюс-минус нормально,
         * а при переносе в код, превратилось в очень громозкие вычисления
         * После небольших раздумий пришёл к решению. Мы подсчитывает для каждой позиции начиная с нулевой кол-во вероятных значений, кстати это и теорией подкрепленно
         * Если тебе потом это снова понадобится, обязательно почитай Получение перестановки по номеру. В принципе там почти подходит, только нас интересуют 
         * не перестановки, а сочетания.*/
        static string EnumeratingWithoutRepetitions(int basis, int sochetanie, long number) {
            //0 1 2 3 4 5 6 7 8 9 A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
            string answer = String.Empty;
            Dictionary<int, string> x36 = new Dictionary<int, string>{ {0,"0" }, {1,"1" }, {2,"2" }, {3,"3" }, {4,"4" }, {5,"5" },
                     {6,"6" }, {7,"7" }, {8,"8" }, {9,"9" }, {10,"A" }, {11,"B" }, {12,"C" }, {13,"D" }, {14,"E" }, {15,"F" }, {16,"G" },
                     {17,"H" }, {18,"I" }, {19,"J" }, {20,"K" }, {21,"L" }, {22,"M" }, {23,"N" }, {24,"O" }, {25,"P" }, {26,"Q" }, {27,"R" },
                     {28,"S" }, {29,"T" }, {30,"U" }, {31,"V" }, {32,"W" }, {33,"X" }, {34,"Y" }, {35,"Z" }
            };
            int[] array = new int[sochetanie];
            //if sochetnie >= basis return basis string
            if (sochetanie >= basis) {
                for (var i = 0; i < basis; i++)
                    answer += $"{x36[i]}";
                return answer;
            }
            for (var i = 0; i < sochetanie; i++)
                array[i] = i;
            int last = 0;

            //c этой частью пришлось повозиться, но в итоге всё стало окей. Идём слева направо. Сначала высчитываем кол-во сочетаний
            //В которых на месте i может быть символ it. Если необходимое число больше, значит, что нас интересует число побольше на позиции i, После того
            // Как находим такое it, при котором количество пройденных наборов будет больше числа, указываем это it на позиции i, и идём дальше
            for(var i=0; i < sochetanie; i++)
                for(var it = last; it<=basis; it++) {
                        long numberPer = NumberOfPermutation(basis - it-1, sochetanie - i-1);
                        if (number < numberPer) {
                        array[i] = it;
                        last = it+1;
                        break;
                    } else number -= numberPer;
                }
            for (var i = 0; i < sochetanie; i++)
                answer += $"{ x36[array[i]]}";
           
            return answer;
        }


        //Метод для генерации всех возможных сочетаний длиной m из множества n. Возращает массив индексов в лексиграфическом порядке
        static List<int[]> GenerationCombination(int n, int m) {
            int[] temp = new int[m];
            List<int[]> answer = new List<int[]>();
            //создаём базовое сочетание
            for(var i =0; i< m; i++) 
                temp[i] = i;
            answer.Add(temp);
            long perm = NumberOfPermutation(n, m);
            //идём с конца, забиваем полный бак на последнем из достпуных элементов, потом переносим разряд и по новой
            while (perm-->0) {
                temp = CloneArr(temp);
                for (var i = m - 1; i >= 0; i--) {
                    if (temp[i] < n - m + i) {// После долгих размышлений, я пришёл к выводу, что на i месте не может быть индекса выше, чем n-m+i
                        temp[i]++;
                        for (int j = i; j < m - 1; j++)
                            temp[j + 1] = temp[j] + 1;
                  
                        answer.Add(temp);
                        break;
                    }

                }
            }

            return answer;
        }


        static Dictionary<(int n, int k), long> numbersPerms = new Dictionary<(int, int), long>();
        //изначально фунция не была рекурсивной
        static long NumberOfPermutation(int n, int k) {

            if (numbersPerms.ContainsKey((n, k)))
                return numbersPerms[(n, k)];
            if ((n == k) || (k == 0)) {
                numbersPerms[(n, k)] = 1;
                return 1;
            }
            if (k == 1) {
                numbersPerms[(n, k)] = n;
                return n;
            }
            numbersPerms[(n, k)] = NumberOfPermutation(n - 1, k) + NumberOfPermutation(n - 1, k - 1);
            return numbersPerms[(n, k)];
        }
    }
}
