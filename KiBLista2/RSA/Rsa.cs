using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RSA
{
    class Rsa
    {
        private BigInteger n;
        private BigInteger d;
        private BigInteger e;

        private int _BitLength;

        public Rsa(int bits)
        {
            _BitLength = bits;

            BigInteger p = new BigInteger(879190841);
            BigInteger q = new BigInteger(817504253);

            n = BigInteger.Multiply(p, q);
            BigInteger eulerLeftSideOfMultiplay = BigInteger.Subtract(p, BigInteger.One);
            BigInteger eulerRightSideOfMultiplay = BigInteger.Subtract(q, BigInteger.One);
            BigInteger eulerResult = BigInteger.Multiply(eulerLeftSideOfMultiplay, eulerRightSideOfMultiplay);

            e = new BigInteger(3);

            while (BigInteger.GreatestCommonDivisor(e, eulerResult) > 1)
            {
                e = BigInteger.Add(e, new BigInteger(2));
            }

            d = ModInverse(e, eulerResult);
        }

        public BigInteger Encrypt(BigInteger msg)
        {
            return BigInteger.ModPow(msg, e, n);
        }

        public string Encrypt(string msg)
        {
            var bytes = Encoding.UTF8.GetBytes(msg);
            BigInteger bigInt = new BigInteger(bytes);
            return BigInteger.ModPow(bigInt,e,n).ToString();
        }

        public BigInteger Decrypt(BigInteger msg)
        {
            return BigInteger.ModPow(msg, d, n);
        }

        public string Decrypt(string msg)
        {
            var bytes = Encoding.UTF8.GetBytes(msg);
            BigInteger bigInt = new BigInteger(bytes);
            return System.Text.Encoding.Default.GetString(BigInteger.ModPow(bigInt, d, n).ToByteArray());

        }


        private BigInteger ModInverse(BigInteger a, BigInteger n)
        {
            BigInteger i = n, v = 0, d = 1;
            while (a > 0)
            {
                BigInteger t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
    }
}
