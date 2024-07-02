using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SecurityLibrary.DiffieHellman
{


    public class DiffieHellman
    {

        public int power(int f, int s, int sf)
        {
            int result = 1;
            for (int i = 0; i < s; i++)
            {
                result *= f;
                result = result % sf;
            }
            return result;
        }

        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            int yA = power(alpha, xa, q);
            int K = power(yA, xb, q);
            List<int> result = new List<int> { K, K };
            return result;
        }
    }
}