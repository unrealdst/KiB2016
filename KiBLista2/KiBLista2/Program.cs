using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



///Many TIme Pad attack
namespace KiBLista2
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = "C:\\Users\\Cebulotron3000\\Documents\\KiBi\\lista2.txt";
            string alphabet = "aąbcćdeęfghijklłmnńoóprsśtuwyzźżAĄBCĆDEĘFGHIJKLŁMNŃOÓPRSŚTUWYZŹŻqQvVxXyY  ,./;'[]\"\\<>?:{}|!@#$%^&*()_+-=";

            var zad2 = new Zad2(path);
            var result = new List<Result>();

            foreach (char c in alphabet)
            {
                result.AddRange(zad2.DoWorks((byte)c));
            }

            result = result.OrderBy(x => x.iterator).ToList();

            result.ForEach(x => x.Check());


            string[] plains = new string[20];

            int last = 0;
            for (int i = 0; i < plains.Length; i++)
            {
                for (int j = 0; j < result.Count; j++)
                {
                    bool b = true;

                    try
                    {
                        b = result[j].iterator == result[j - 1].iterator;
                    }
                    catch (ArgumentOutOfRangeException) { }

                    if (result[j].isGood && b)
                    {
                        plains[i] = plains[i] + (char)result[j].result[i];
                    }
                }
            }

            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].isGood)
                {
                    Console.WriteLine(String.Format(">{0}< {1}", (char)result[i].sing, result[i].iterator));
                    foreach (byte b in result[i].result)
                    {
                        Console.Write((char)b);
                    }
                    Console.Write("\n");
                }
            }
            Console.Write("\n"); Console.Write("\n"); Console.Write("\n");
            foreach (string s in plains)
            {
                Console.WriteLine(s + "\n\n\n\n");
            }
        }
    }

    class Zad2
    {
        private string _path;

        public Zad2(string path)
        {
            this._path = path;
        }

        public List<Result> DoWorks(byte sign)
        {
            List<byte[]> cyphers = ReadFile();
            List<Result> finalResult = new List<Result>(16);

            int minLength = FindMinLength(cyphers);

            for (int i = 0; i < minLength; i++)
            {
                byte thisCouldBeAKey = Xor(cyphers[0][i], sign);
                byte[] result = new byte[cyphers.Count];
                var res = new Result()
                {
                    iterator = i,
                    sing = sign,
                    result = new byte[cyphers.Count]
                };

                for (int j = 0; j < cyphers.Count; j++)
                {

                    res.result[j] = Xor(thisCouldBeAKey, cyphers[j][i]);
                }
                finalResult.Add(res);
            }

            return finalResult;
        }

        private byte Xor(byte p, byte q)
        {
            string a = Convert.ToString(p, 2);
            string b = Convert.ToString(q, 2);

            FillStringto8Length(ref a);
            FillStringto8Length(ref b);

            string result = "";

            for (int i = 0; i < Math.Max(a.Length, b.Length); i++)
            {
                result = result + charXor(a[i], b[i]);
            }


            return Convert.ToByte(result, 2);
        }

        private void FillStringto8Length(ref string a)
        {
            for (int i = a.Length; i < 8; i++)
            {
                a = "0" + a;
            }
        }

        private string charXor(char p, char q)
        {
            return (ConvertToBool(p) ^ ConvertToBool(q)) ? "1" : "0";
        }

        private bool ConvertToBool(char c)
        {
            return c == '1';
        }

        private int FindMinLength(List<byte[]> cyphers)
        {
            int min = cyphers.First().Count();
            foreach (byte[] cypher in cyphers)
            {
                if (cypher.Count() < min)
                {
                    min = cypher.Count();
                }
            }
            return min;
        }

        private List<byte[]> ReadFile()
        {
            string line;
            List<byte[]> cypher = new List<byte[]>();
            System.IO.StreamReader file = new System.IO.StreamReader(_path);

            while ((line = file.ReadLine()) != null)
            {
                var signsInByte = new List<byte>();

                List<string> tempSings = line.Split(' ').ToList();
                tempSings.RemoveAll(x => x.Count() != 8);

                string[] signs = tempSings.ToArray();

                foreach (string s in signs)
                {
                    signsInByte.Add(Convert.ToByte(s, 2));
                }

                var oneLineBytes = signsInByte.ToArray();

                cypher.Add(oneLineBytes);
            }

            file.Close();

            return cypher;
        }
    }

    class Result
    {
        public byte sing { get; set; }
        public int iterator { get; set; }
        public byte[] result { get; set; }
        public bool isGood { get; private set; }

        private string alphabet = "aąbcćdeęfghijklłmnńoóprsśtuwyzźżAĄBCĆDEĘFGHIJKLŁMNŃOÓPRSŚTUWYZŹŻqQvVxXyY .,1234567890!?";

        //private string alphabet = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM,./;'[]\"\\<>?:{}|!@#$%^&*()_+-=";

        public bool Check()
        {
            foreach (byte b in result)
            {
                if (!alphabet.Contains((char)b))
                {
                    isGood = false;
                    return false;
                }
            }
            isGood = true;
            return true;
        }


    }
}
