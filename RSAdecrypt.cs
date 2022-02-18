using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RSA_Crypto {
    class RSACryptoFunc {
        BigInteger m_cypherMessage;
        BigInteger p;
        BigInteger q;
        BigInteger n;
        BigInteger phiN;
        BigInteger d;
        BigInteger e;
        public RSACryptoFunc(string pIn, string qIn, string cypherMessage) {
            m_cypherMessage = BigInteger.Parse(cypherMessage);
            p = BigInteger.Parse(pIn);
            q = BigInteger.Parse(qIn);
            n = p * q;
            phiN = n - p - q + 1;
            e = 65537;
            ModEquation temp = new ModEquation(e, -1, phiN);
            d = temp.SolutionOfEquation();
            Console.WriteLine($"n = {n}\nphi(n) = {phiN} \nd = {d}");
        }
        public string GiveDecypher() {
            /*Console.WriteLine("TEST");
            string testing = Console.ReadLine();
            BigInteger value1 = 0;
            for (var i = 0; i < testing.Length; i++) {
                value1 *= 100;
                value1 += (int)testing[i];
            }
            Console.WriteLine($"This's you message\n{value1};");// this is right, go next; without modul its a bad idea
            value1 = PowBIexponent(value1, e, n);
            Console.WriteLine($"This's you message after encrypt with module\n {value1}");
            value1 = PowBIexponent(value1, d, n);
            Console.WriteLine($"This is you message after decrypt with module\n{value1}");
            Console.WriteLine($"Check for normal working n,p,q\n p*q%euler(n) = {e*d % phiN}");
            return "";*/
             BigInteger test = PowBIexponent(m_cypherMessage, d,n);
            Console.WriteLine(test);
             return $"{test}";
        }
        public string DecodeDecypher(string input) {
            string answer=string.Empty;
            for(var i = input.Length-1; i>=0; i--) {
                if(input[i-1]== input[i] && input[i] == '0') {
                    input = input.Remove(i - 1);
                    break;
                }
            }
            for (var i = 0; i < input.Length-1; i += 2) {
                answer += (char)Int32.Parse(input.Substring(i, 2));
            }
            return answer;
        }
        //Ха.Ха-ха.Хахахахаха. Думал обойдётся без возведений в степень через двоичную степень? Ни**я. Более того, не просто так е = 65537, так как там всего две 
        //единички, значит явно для того, чтобы быстрее возводить в степень при шифровании, ПРОБЛЕМА В ТОМ, ЧТО МЫ ДЕШИФРУЕМ(= Искать старое решение не хочется
        //Напишу заново, плюс восстановлю всё это дело в памяти.
        static List<int> ConvertToBin(BigInteger number) { // 
            List<int> binNumber = new List<int>();
            BigInteger tmp = number;
            for (var i = 0; ; i++) {
                if (tmp % 2 == 0) {
                    binNumber.Add(0);
                    tmp /= 2;
                } else {
                    binNumber.Add(1);
                    tmp = (tmp - 1) / 2;
                }
                
                if (tmp == 0)
                    return binNumber;
            }
        }
        //Если будет медленно считать добавим модуль.
        public static BigInteger PowBIexponent(BigInteger baseNumber, BigInteger exp,BigInteger modul) {

            //Получаем обратную запись двоичного числа exp
            List<int> binExp = ConvertToBin(exp);
            BigInteger answer = 1;
            //Возводим в степень в начале цикла, чтобы не пришлось делать дополнительную строку вне цикла, где пришлось бы answer умножать на базу^младший бит степени
            for(var i = 0; i < binExp.Count; i++) {
                if (modul != -1) {
                    answer = BigInteger.Pow(answer, 2)%modul;
                    answer *= BigInteger.Pow(baseNumber, binExp[binExp.Count - 1 - i]);
                    answer %= modul;
                }
                else {
                    answer = BigInteger.Pow(answer, 2);
                    answer *= BigInteger.Pow(baseNumber, binExp[binExp.Count - 1 - i]);
                }
            }
            // в принципе если сейчас всё сработает, то дальнейшая часть расшифровки RSA - нехитрое дело(= просто скучное и только(
            return answer;
        }
    }
}
