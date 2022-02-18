using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//популярный алгоритм для определения рейтинга страниц
namespace OrientedGraphTwo {
    class PageRank :OrientedGraphTwo.OrientedGraph{
        List<double> m_value;
        public PageRank() :base(){
            m_value = new List<double>();
        }
        public PageRank(int numberOfPeeks) : base() {
            m_value = new List<double>();
            for(var i =0; i < numberOfPeeks; i++) {
                this.AddFreePeek($"{i}");
                m_value.Add(0);
            }
            
        }
        public int[] WalkingFuckingWay(int walkers,int steps) {
            int[] answer = new int[m_value.Count];
            for(var j =0; j < m_value.Count;j++)
                for(var i=0; i < walkers; i++) {
                    int endPeek = WalkingThrowPages(j, steps);
                    answer[endPeek]++;
                }
            return answer;
                    
        }
        
        int WalkingThrowPages(int startPeek, int steps) {
            if (steps == 0)
                return startPeek;
            List<(double,int)> chanceOfWay = new List<(double,int)>();
            for(var i =0; i < m_value.Count; i++) {
                if (m_matrix[startPeek, i] == 1)
                    chanceOfWay.Add((m_value[i]*100, i));
            }

            Random myRand = new Random();
            int sum = (int)chanceOfWay.Sum(x=>(int)x.Item1);
            int dice = myRand.Next(sum);
            int testedValue = 0;
            for (var i = 0; i < chanceOfWay.Count; i++) {
                testedValue += (int)chanceOfWay[i].Item1;
                if (dice <= testedValue)
                    return WalkingThrowPages(chanceOfWay[i].Item2, steps - 1);
            }
            return WalkingThrowPages(chanceOfWay[chanceOfWay.Count - 1].Item2, steps - 1);
            
        }
        //функция для использования юзером, на поиск рангов страниц
        public void PageRankMain() {
            for(var i=0; i < m_value.Count; i++) 
                m_value[i] = 1.0 / m_value.Count;
            List<double> newRank = PageRankCycle();
            bool checkIn = false;
            const double epsilon = 0.001;
            while (!checkIn) {
                checkIn = true;
                for(var i =0; i < m_value.Count; i++) {
                    if (Math.Abs(newRank[i] - m_value[i]) > epsilon) {
                        checkIn = false;
                        break;
                    }
                }
                m_value = newRank;
                newRank = PageRankCycle();
                if (checkIn)
                    break;
            }
            double controlSum = 0;
            foreach (var member in newRank) {
                Console.Write($"{member} ");
                controlSum += member;
            }
            Console.WriteLine(controlSum);
            
        }
        //сходящийся цикл для всех вершин
        List<double> PageRankCycle() {
            List<double> answer = new List<double>(m_value) ;
            for (var i = 0; i < m_value.Count; i++)
                answer[i] = OnePageRank(i, answer);
            return answer;
        }

        //часть цикла для одной вершины
        //Сходящийся алгоритм нашёлся случайным образом на просторах интернета(=
        /* 
5 10  
0 4
0 2
1 0
1 4
2 0
2 1
2 4
2 3
3 4
4 2
         * */
        double OnePageRank(int numberPage, List<double> input) {
            const double d = 0.85;
            double first = (1 - d) / input.Count;
            double second = 0;
            for(var i=0; i < input.Count; i++) {
                if (m_matrix[numberPage,i] == 1) {
                    int count = 0;
                    for (var j = 0; j < input.Count; j++)
                        if (m_matrix[j, i] == 1)
                            count++;
                    second += d * (input[i]) / count;
                }
            }
            return first + second;
        }
    }
}
