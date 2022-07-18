using System;
using System.Collections.Generic;


//Тестовое задание для компании Stenn с тестами для проверки, что я не продебилился.
namespace StennSoultion {
    class Program {
        static void Main(string[] args) {
            int x = 12;
            int y = 34;
            int z = 126;
            ArithmeticParticularSwap(ref x, ref y, ref z);
            Console.WriteLine($"x = {x} \n y = {y} \n z = {z}");
            Car car1, car2;
            car1 = new Car();
            car1.Color = "Green glass";
            car1.Model = "Pontiac750";

            car2 = new Car();
            car2.Color = "Green glass";
            car2.Model = "Pontiac750";

            Car twentyOne = new Car();
            twentyOne.Color = "Red Eva";
            twentyOne.Color = "Skyline";
            Console.WriteLine($"\n equals? {car1.Equals(car2)} \n Hash? car1 = {car1.GetHashCode()} \n car2 = {car2.GetHashCode()} \n" +
                $"car3 = {twentyOne.GetHashCode()}");

            Tree testTree = new Tree();
            testTree.Name = "_*-*_";
            testTree.Children = new List<Tree>();
            Tree currentTree = testTree;
            for(var i = 0; i < 4; i++) {
                for(var j = 0; j < 6; j++) {
                    currentTree.Children.Add(new Tree());
                    currentTree.Children[j].Name = $"[i*j = {i * j}\n";
                    currentTree.Children[j].Children = new List<Tree>();
                }
                currentTree = currentTree.Children[i];
            }
            List<string> nameChildren = testTree.GetNameAllChildrens();
            foreach (var member in nameChildren)
                Console.WriteLine(member);


        }

        /*Заданы три целочисленные переменные с начальными значениями X=X0, Y=Y0, Z=Z0. 
         * Поменяйте значения этих переменных так, чтобы X=Y0, Y=Z0, Z=X0. 
         * Необходимо написать метод, реализующий алгоритм обмена, без использования дополнительных переменных кроме X,Y,Z.
         * */
        public static void ArithmetiGlobalcSwap(ref int x, ref int y) {
            x += y;
            y = x - y;
            x -= y;
        }
        public static void ArithmeticParticularSwap(ref int x, ref int y, ref int z) {
            ArithmetiGlobalcSwap(ref x, ref y);
            ArithmetiGlobalcSwap(ref y, ref z);

        }

        /*У автосалона в распоряжении есть парк автомобилей (задана коллекция экземпляров класса Car; Model - марка и модель авто;
         * Color - его цвет; Description - описание особенностей экземпляра). При этом особенности автомобиля автосалону не важны, 
         * менеджер различает автомобили лишь по марке, модели и цвету. Напишите переопределение Equals и GetHashCode для класса так,
         * чтобы оно наилучшим образом соответствовало условиям задачи.
         */
        public class Car {
            public string Model { get; set; }
            public string Color { get; set; }
            public string Description { get; set; }


            //не очень люблю через исключения работать, но за неимением горничной...(=
            public override bool Equals(object obj) {
                if (obj == null) {
                    throw new Exception("empty space. i Found them!");
                }
                if (this.GetType() != obj.GetType()) {
                    throw new Exception("Hm... This isn't car");
                }
                Car tmpCar = (Car)obj;
                if (this.Model == tmpCar.Model && this.Color == tmpCar.Color)
                    return true;
                return false;
            }
            public override int GetHashCode() {
                return String.GetHashCode(this.Model) + String.GetHashCode(this.Color);
            }
        }


        //Есть  дерево. Реализовать алгоритм, который возвращает все имена Name, содержащиеся в этом дереве.
        //Видимо, это очень плодоносное дерево и у него много нодов на одной ветке
        //

        public class Tree {

            public string Name { get; set; }

            public List<Tree> Children { get; set; }




            //Задаётся с нода, чьи дети интересуют.
            //Далее, чтобы без рекурсии, берём все текущие ноды
            //Закидываем их в очередь, вытаскиваем по одному, добавляя к ответу
            //При доставании с очереди мы добавляем всех детей вытащенного молодца в очередь.
            public List<string> GetNameAllChildrens() {

                List<string> answer = new List<string>();
                Queue<Tree> nodesOfTree = new Queue<Tree>();

                foreach (var member in this.Children) {
                    nodesOfTree.Enqueue(member);
                }

                while (nodesOfTree.Count > 0) {
                    Tree currentNode = nodesOfTree.Dequeue();
                    answer.Add(currentNode.Name);
                    foreach (var member in currentNode.Children)
                        nodesOfTree.Enqueue(member);
                }

                return answer;
            }


        }
    }
}
