using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NormalBinaryTree {

    //полноценное шаблонное дерево
    class BinaryTree<T> where T: IComparable {

        protected T m_value;
        protected BinaryTree<T> m_parent, m_left, m_right;

        public BinaryTree(T value, BinaryTree<T> parent = null) {
            m_value = value;
            m_parent = parent;
        }
            
        //Возвращает коль-во родителей до корня.
        public int GiveNumberParents() {
            BinaryTree<T> temp = this;
            int number = 0;
            while (temp.m_parent != null) {
                number++;
                temp = temp.m_parent;
            }
            return number;
        }
        //Возвращает родителя, если тот есть, в обратном случае возвращает самого себя
        public T GiveLastParent() {
            if (this.m_parent != null)
                return this.m_parent.GiveLastParent();
            else return this.m_value;
        }
        //Обычная функция добавления
        public void Add(T value) {
            if (this.m_value.CompareTo(value) > 0) // Если входящие данные меньше или равны m_value, то данные идут в правый нод
                if (this.m_left == null)
                    this.m_left = new BinaryTree<T>(value, this);
                else
                    this.m_left.Add(value);
            else if (this.m_right == null)
                this.m_right = new BinaryTree<T>(value, this);
            else this.m_right.Add(value);
        }

        private BinaryTree<T> SearchTree(BinaryTree<T> Tree, T value) {// сделай утром дубликат SearchTree2 и замени Tree на This. Может проблема в передаче?
            if (Tree == null)
                return null;
            switch (Tree.m_value.CompareTo(value)) {
                case 1: return SearchTree(Tree.m_left, value);
                case -1: return SearchTree(Tree.m_right, value);
                case 0: return Tree;
                default: return null;
            }
        }
        //функция поиска для юзера
        public BinaryTree<T> Search(T value) { return SearchTree(this, value); }
        //удаление лепестка
        public bool Remove(T value) {
            BinaryTree<T> removeNode = Search(value);
            if (removeNode == null)
                return false;

            if (removeNode.m_left == null && removeNode.m_right == null) {//esli eto list
                if (removeNode.m_parent.m_left == removeNode)
                    removeNode.m_parent.m_left = null;
                else
                    removeNode.m_parent.m_right = null;
                return true;
            }
            if (removeNode.m_left == null || removeNode.m_right == null) {// esli 1 iz detei est. varian oba null rassmotren vishe
                if (removeNode.m_left == null) {
                    if (removeNode.m_parent.m_left == removeNode)
                        removeNode.m_parent.m_left = removeNode.m_right;
                    else removeNode.m_parent.m_right = removeNode.m_right;
                } else {
                    if (removeNode.m_parent.m_left == removeNode)
                        removeNode.m_parent.m_left = removeNode.m_left;
                    else removeNode.m_parent.m_right = removeNode.m_left;
                }
            }
            //остаётся ситуация, где у нода два ребёнка. Тогда берём самого маленького из правого  поддерева
            BinaryTree<T> tempTree;
            tempTree = removeNode.m_right;
            while (tempTree.m_left != null)
                tempTree = tempTree.m_left;
            tempTree.m_left = removeNode.m_left;
            if (removeNode.m_parent.m_left == removeNode)
                removeNode.m_parent.m_left = tempTree;
            else removeNode.m_parent.m_right = tempTree;
            return true;
        }
        public void PrintTree() {
            Console.WriteLine(Liberty());
        }
        private string Liberty() {
            string temp = $"(,{this.m_value},)";
            if (m_left != null)
                temp = temp.Insert(1, m_left.Liberty());
            else temp = temp.Insert(1, "-");
            if (m_right != null)
                temp = temp.Insert(temp.Length - 1, m_right.Liberty());
            else temp = temp.Insert(temp.Length - 1, "-");
            return temp;
        }
        //Функция, которая будет возвращать хэштаблицу символ-строка(строка состоит из I и O, через буквы показывая двоичный код, через который символ выражен)
        //предположим пока что, что оно работает.
        public Dictionary<char, string> ListValues() {
            Dictionary<char, string> answer = new Dictionary<char, string>();
            if(this.m_left != null) {
                Dictionary<char, string> temp = this.m_left.ListValues();
                foreach (var member in temp)
                    answer.Add(member.Key, member.Value);
            }
            if(this.m_right != null) {
                Dictionary<char, string> temp = this.m_right.ListValues();
                foreach (var member in temp)
                    answer.Add(member.Key, member.Value);
            }
            if(this.m_right ==null && this.m_left == null) {
                answer.Add($"{this.m_value}"[0], "");
            }
            if (this.m_parent != null) {
                List<char> azbuka = answer.Select(u => u.Key).ToList();
                string insert = "2";//for test
                if (this.m_parent.m_left == this)
                    insert = "O";
                if(this.m_parent.m_right== this)
                    insert = "I";
                if(insert !="2")
                    foreach (var member in azbuka)
                        answer[member] = answer[member].Insert(0, insert);
            }
            return answer;
            
                


        }
    }

    //Пока я вспомнил старую инфу десятилетней давности, что можно наследовать от шаблонных классов, уже все проклял(=
    class BinaryTreeHoffman : BinaryTree<NodeInfo> {
        
        public BinaryTreeHoffman(NodeInfo value, BinaryTreeHoffman parent=null) : base(value, parent) { }

        //сначала мне показалась эта функция глупой. Спустя месяц она мне такой не кажется, все аккуратненько.
        //нод, который будет иметь ветки не имеет права иметь чар значение, но будет иметь общий вес двух подключенных к нему деревьев. Всё аккуратненько. Не значю,
        // что я так рефлексировал. first -> to left, second -> to right
        public static BinaryTreeHoffman ConnectTwoNodeInfo(BinaryTreeHoffman first, BinaryTreeHoffman second) {
            int freq = first.GiveLastParent().m_freq + second.GiveLastParent().m_freq;
            BinaryTreeHoffman newParent = new BinaryTreeHoffman(new NodeInfo(freq));
            newParent.m_left = first;
            newParent.m_right = second;
            first.m_parent = newParent;
            second.m_parent = newParent;
            return newParent;
        }


        //Кстати, можно было сделать через Add в дереве, но захотелось на верочку, столько строк кода, можно не уследить ошибку.
        static public BinaryTreeHoffman RecursiveInsertion(List<(char name, int freq)> inputList, BinaryTreeHoffman parent) {
            BinaryTreeHoffman answer = new BinaryTreeHoffman(new NodeInfo(0),parent);
            if (inputList.Count == 1) {
                //answer.m_left = new BinaryTreeHoffman(new NodeInfo(inputList[0].freq, inputList[0].name), answer);
                answer.m_value = new NodeInfo(inputList[0].freq, inputList[0].name);

            }
            else if (inputList.Count == 2) {
                answer.m_left = new BinaryTreeHoffman(new NodeInfo(inputList[0].freq, inputList[0].name),answer);
                answer.m_right = new BinaryTreeHoffman(new NodeInfo(inputList[1].freq, inputList[1].name), answer);
            }
            else {
                int iterator = FindIterator(inputList);
                List<(char name, int freq)> left = new List<(char name, int freq)>(),
                                            right = new List<(char name, int freq)>();
                for (var i = 0; i <= iterator; i++)
                    left.Add(inputList[i]);
                for (var i = iterator + 1; i < inputList.Count; i++)
                    right.Add(inputList[i]);
                answer.m_left = RecursiveInsertion(left,answer);
                answer.m_right = RecursiveInsertion(right, answer);
            }
            return answer;
        }
        //поиск указателя. Указывает на последний элемент, который остается в левой части
        static int FindIterator(List<(char name, int freq)> inputList) {
            int left=0, right=0, last, iterator = 0;
            foreach (var member in inputList)
                right += member.freq;
            while (true) {
                last = Math.Abs(right-left);
                left += inputList[iterator].freq;
                right -= inputList[iterator].freq;
                Console.WriteLine($"Iterator {iterator} left side {left} right{right} last {last}");
                Console.WriteLine($"last == {last} new {Math.Abs(right - left)}");
                if (Math.Abs(right - left) < last)
                    iterator++;
                else return iterator-1;

            }
        }



    }
}
