using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            int a = baseN;
            int b = number;
            int x0 = 0, x1 = 1, y0 = 1, y1 = 0;

            while (b != 0)
            {
                int quotient = a / b;
                int remainder = a % b;

                a = b;
                b = remainder;

                int tempX = x1;
                x1 = x0 - quotient * x1;
                x0 = tempX;

                int tempY = y1;
                y1 = y0 - quotient * y1;
                y0 = tempY;
            }

            if (a != 1)
                return -1;
            else
                return (x0 % baseN + baseN) % baseN;
            
        }
    }
}
