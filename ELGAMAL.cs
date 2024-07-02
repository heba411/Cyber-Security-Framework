using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        /// 

        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            List<long> result = new List<long>(2);
            BigInteger K = BigInteger.ModPow(y, k, q);
            BigInteger c1 = BigInteger.ModPow(alpha, k, q);
            BigInteger c2 = (K * m) % q;
            long C1 = (long)c1;
            long C2 = (long)c2;
            result.Add(C1);
            result.Add(C2);
            return result;

        }

        public int Decrypt(int c1, int c2, int x, int q)
        {
            BigInteger K = BigInteger.ModPow(c1, x, q);
            int k = (int)K;
            BigInteger inv = findInv(k, q);
            BigInteger m = (c2 * inv) % q;
            int M = (int)m;
            return M;

        }

        public int findInv(int b, int m)
        {
            int i = m;
            int inv = 0; 
            int d = 1;
            while (b > 0)
            {
                int t = i / b;
                int x = b;
                b = i % x;
                i = x;
                x = d;
                d = inv - t * x;
                inv = x;
            }
            inv %= m;
            if (inv < 0)
            {
                inv = (inv + m) % m;
            }
            return inv;
        }
    }
}
