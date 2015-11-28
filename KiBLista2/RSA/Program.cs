using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            var Rsa = new Rsa(1024);

            var bytes = Encoding.UTF8.GetBytes(Resource.LoremIpsum);
            BigInteger bigInt = new BigInteger(bytes);


            BigInteger cipher = Rsa.Encrypt(bigInt);

            BigInteger plain = Rsa.Decrypt(cipher);


            var result = plain.ToByteArray();

            bool succed = (bytes == result);
           // char[] charsValues = plain.ToByteArray().Select(b => (char)b).ToArray();

          //  string result = new String(charsValues);
          //  string res2 = System.Text.Encoding.Default.GetString(plain.ToByteArray());
        }
    }
}