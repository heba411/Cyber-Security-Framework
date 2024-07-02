using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int get(int e, int z)
        {
            long c = 1000000000000000000;
            int b = 0;
            if (e < 0)
            {
                e = (e % z) + z;
            }
            // b*D mod z = 1 --> find b
            for (int i = 1; i <= c; i++)
            {
                if ((i * e) % z == 1)
                {
                    b = i;
                    break;
                }
                else
                    continue;
            }
            return b;
        }
        public int[] key_generation(int p, int q, int e)
        {
            int[] a = new int[2];
            int n = p * q;
            int z = (p - 1) * (q - 1);
            Console.WriteLine("z=" + z);
            int d = get(e, z);
            a[0] = n;
            a[1] = d;
            return a;
        }

        public BigInteger pow(long b, int power)
        {
            BigInteger res = 1;
            for (int i = 0; i < power; i++)
            {
                res *= b;
            }
            return res;
        }

        public int Encrypt(int p, int q, int M, int e)
        {
            int[] a = key_generation(p, q, e);
            int n = a[0];
            int d = a[1];
            Console.WriteLine("n=" + n);
            Console.WriteLine("d=" + d);
            BigInteger h = pow((long)M, e);
            Console.WriteLine("h=" + h);
            BigInteger cipher = h % n;
            Console.WriteLine("cipher=" + cipher);
            return (int)cipher;
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            int[] a = key_generation(p, q, e);
            int n = a[0];
            int d = a[1];
            Console.WriteLine("n=" + n);
            Console.WriteLine("d=" + d);
            BigInteger h = pow((long)C, d);
            Console.WriteLine("h=" + h);
            BigInteger plainText = h % n;
            Console.WriteLine("plainText=" + plainText);
            return (int)plainText;
        }
    }
}
