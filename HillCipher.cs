using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{

    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {
        public int[,] createKeyMatrix(List<int> key)
        {
            int size = (int)Math.Sqrt(key.Count);
            //Console.WriteLine(key.Count);
            //Console.WriteLine(size);

            int[,] keyMatrix = new int[size, size];
            for (int i = 0; i < key.Count; i++)
            {
                keyMatrix[i / size, i % size] = key[i];
            }

            //for (int i = 0; i < size; i++)
            //{
            //    for (int j = 0; j < size; j++)
            //    {
            //        Console.Write(keyMatrix[i, j] + "\t");
            //    }
            //    Console.WriteLine();
            //}

            return keyMatrix;
        }
        public int[,] createMatrix(List<int> plain, int lengthOfRows)
        {

            int cols = (int)Math.Ceiling((double)plain.Count / lengthOfRows);
            int[,] keyMatrix = new int[lengthOfRows, cols];
            for (int i = 0; i < plain.Count; i++)
            {
                keyMatrix[i % lengthOfRows, i / lengthOfRows] = plain[i];
            }

            //for (int i = 0; i < lengthOfRows; i++)
            //{
            //    for (int j = 0; j < cols; j++)
            //    {
            //        Console.Write(keyMatrix[i, j] + "\t");
            //    }
            //    Console.WriteLine();
            //}

            return keyMatrix;
        }
        public int[,] multiply(int[,] key, int[,] plain)
        {
            int rowsPlain = plain.GetLength(0);
            int colsPlain = plain.GetLength(1);
            int rowsKey = key.GetLength(0);
            int colsKey = key.GetLength(1);

            int[,] result = new int[rowsPlain, colsPlain];
            if (colsKey != rowsPlain)
            {
                throw new ArgumentException("Invalid matrix dimensions for multiplication");
            }
            for (int i = 0; i < rowsKey; i++)
            {
                for (int j = 0; j < colsPlain; j++)
                {
                    for (int k = 0; k < colsKey; k++)
                    {
                        result[i, j] += key[i, k] * plain[k, j];
                    }
                }
            }

            //for (int i = 0; i < result.GetLength(0); i++)
            //{
            //    for (int j = 0; j < result.GetLength(1); j++)
            //    {
            //        Console.Write(result[i, j]%26 + "\t");
            //    }
            //    Console.WriteLine();
            //}
            return result;
        }
        public int calcDeterminant2x2(int a, int b, int c, int d)
        {
            int res = (a * d) - (b * c);
            return res;
        }
        public int calcDeterminant3x3(int[,] matrix)
        {
            int res = 0;
            for (int i = 0; i < 3; i++)
            {
                res += matrix[0, i] * calcDeterminant2x2(matrix[1, (i + 1) % 3],//a
                                                        matrix[1, (i + 2) % 3],//b
                                                        matrix[2, (i + 1) % 3],//c
                                                        matrix[2, (i + 2) % 3]);//d
            }
            return res;
        }
        public int[,] getCofactor(int[,] A, int row, int column)
        {
            int[,] minorMatrixAfterRemovingRowAndColumn = new int[2, 2];
            int r = 0, c;
            for (int i = 0; i < 3; i++)
            {
                if (i == row)
                    continue;
                c = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (j == column)
                        continue;
                    minorMatrixAfterRemovingRowAndColumn[r, c] = A[i, j];
                    c++;
                }
                r++;
            }
            return minorMatrixAfterRemovingRowAndColumn;
        }
        public int[,] adj(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[,] adjointMatrix = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int sign = ((i + j) % 2 == 0) ? 1 : -1;
                    int[,] temp = getCofactor(matrix, i, j);
                    adjointMatrix[i, j] = sign * calcDeterminant2x2(temp[0, 0], temp[0, 1], temp[1, 0], temp[1, 1]);
                }
            }
            return adjointMatrix;
        }
        public int get(int D)
        {
            int c = 50;
            int b = 0;
            if (D < 0)
            {
                D = (D % 26) + 26;
            }
            // b*D mod 26 = 1 --> find b
            for (int i = 1; i <= c; i++)
            {
                if ((i * D) % 26 == 1)
                {
                    b = i;
                    break;
                }
                else
                    continue;
            }
            return b;
        }
        public int[,] getInverseOfMatrix(List<int> Key)
        {
            Console.WriteLine("Key Matrix");
            int[,] key = createKeyMatrix(Key);
            int rowsKey = key.GetLength(0);
            int colsKey = key.GetLength(1);
            int[,] inverseMatrix = new int[rowsKey, colsKey];
            if (rowsKey == colsKey)
            {
                // matrix with size 2x2
                if (rowsKey == 2)
                {
                    int a = key[0, 0], b = key[0, 1], c = key[1, 0], d = key[1, 1];
                    int D = 1 / ((a * d) - (b * c));
                    if (D == 0)
                    {
                        throw new InvalidAnlysisException();
                    }
                    inverseMatrix[0, 0] = D * d;
                    inverseMatrix[0, 1] = D * (-b);
                    inverseMatrix[1, 0] = D * (-c);
                    inverseMatrix[1, 1] = D * a;
                }
                // matrix with size 3x3
                else if (rowsKey == 3)
                {
                    int resDet = calcDeterminant3x3(key);
                    if (resDet == 0)
                    {
                        throw new InvalidAnlysisException();
                    }
                    int inverse_resDet = get(resDet);
                    //Console.WriteLine("inverse_D");
                    int[,] adjointMatrix = adj(key);
                    for (int i = 0; i < rowsKey; i++)
                    {
                        for (int j = 0; j < rowsKey; j++)
                        {
                            int k = (inverse_resDet * adjointMatrix[i, j]) % 26;
                            if (k < 0)
                            {
                                k = k + 26;
                            }
                            inverseMatrix[j, i] = k;
                        }
                    }
                }
            }
            Console.WriteLine("Invers Key Matrix");
            for (int i = 0; i < rowsKey; i++)
            {
                for (int j = 0; j < colsKey; j++)
                {
                    Console.Write(inverseMatrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
            return inverseMatrix;
        }
        public int[,] plainInverse(int[,] m)
        {
            int rows = m.GetLength(0);
            int cols = m.GetLength(1);
            int[,] inverseMatrix = new int[rows, cols];
            if (rows == cols)
            {
                // matrix with size 2x2
                if (rows == 2)
                {
                    int a = m[0, 0], b = m[0, 1], c = m[1, 0], d = m[1, 1];
                    int det = ((a * d) - (b * c)) % 26;
                    //Console.WriteLine("det = {0}", det);
                    int D = 1 / det;
                    //Console.WriteLine("1/Det = {0}", D);
                    if (det == 0)
                    {
                        throw new InvalidAnlysisException();
                    }
                    inverseMatrix[0, 0] = D * d;
                    inverseMatrix[0, 1] = D * (-b);
                    inverseMatrix[1, 0] = D * (-c);
                    inverseMatrix[1, 1] = D * a;
                }
                // matrix with size 3x3
                else if (rows == 3)
                {
                    int resDet = calcDeterminant3x3(m);
                    if (resDet == 0)
                    {
                        throw new InvalidAnlysisException();
                    }
                    int inverse_resDet = get(resDet);
                    //Console.WriteLine("inverse_D");
                    int[,] adjointMatrix = adj(m);
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            int k = (inverse_resDet * adjointMatrix[i, j]) % 26;
                            if (k < 0)
                            {
                                k = k + 26;
                            }
                            inverseMatrix[j, i] = k;
                        }
                    }
                }
            }
            Console.WriteLine("Invers m Matrix");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(inverseMatrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
            return inverseMatrix;
        }
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {

            //throw new NotImplementedException();
            //throw new InvalidAnlysisException();
            //List<int> pt = new List<int>() { plainText[4], plainText[5], plainText[6], plainText[7] };
            //List<int> c = new List<int>() { cipherText[4], cipherText[5], cipherText[6], cipherText[7] };
            //int[,] ptm = createMatrix(pt, 2);
            //int[,] cm = createMatrix(c, 2);
            //int[,] pti = plainInverse(ptm);
            //int[,] key = multiply(cm, pti);
            //List<int> k = new List<int>();
            //for (int i = 0; i < key.GetLength(0); i++)
            //{
            //    for (int j = 0; j < key.GetLength(1); j++)
            //    {
            //        key[i, j] = key[i, j] % 26;
            //        k.Add(key[i, j]);
            //    }
            //}
            bool found = false;
            List<int> sol = new List<int>();
            for (int a = 0; a < 26; ++a)
            {
                for (int b = 0; b < 26; ++b)
                {
                    for (int c = 0; c < 26; ++c)
                    {
                        for (int d = 0; d < 26; ++d)
                        {
                            List<int> key = new List<int>() { a, b, c, d };
                            List<int> ciph = Encrypt(plainText, key);
                            int count = 0;
                            for (int i = 0; i < ciph.Count; ++i)
                            {
                                if (ciph[i] == cipherText[i])
                                {
                                    count++;
                                }
                            }
                            if (count == cipherText.Count)
                            {
                                sol = key;
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!found)
            {
                throw new InvalidAnlysisException();
            }
            return sol;
        }
        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {

            int[,] inverseKeyMatrix = getInverseOfMatrix(key);
            int rows = (int)Math.Sqrt(key.Count);
            Console.WriteLine("cipher Matrix");
            int[,] cipherMatrix = createMatrix(cipherText, rows);
            Console.WriteLine("Result multipliaction");
            int[,] inverse_X_cipher = multiply(inverseKeyMatrix, cipherMatrix);
            List<int> res = new List<int>();
            for (int i = 0; i < inverse_X_cipher.GetLength(1); i++)
            {
                for (int j = 0; j < inverse_X_cipher.GetLength(0); j++)
                {
                    int m = inverse_X_cipher[j, i] % 26;
                    if (m < 0)
                    {
                        m = 26 + m;
                    }
                    res.Add(m);
                }
            }

            return res;
        }
        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> result = new List<int>();
            int rows = (int)Math.Sqrt(key.Count);
            //Console.WriteLine("Key Matrix: \n");
            int[,] keyMatrix = createKeyMatrix(key);
            //Console.WriteLine("PlainText Matrix: \n");
            int[,] plainTextMatrix = createMatrix(plainText, rows);
            //Console.WriteLine("Cipher Matrix: \n");
            int[,] res = multiply(keyMatrix, plainTextMatrix);

            int row = res.GetLength(0);
            int col = res.GetLength(1);


            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    result.Add(res[j, i] % 26);
                }
            }

            //foreach (int value in result)
            //{
            //    Console.WriteLine(value);
            //}

            return result;
        }
        public string Encrypt(string plainText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {

            //throw new NotImplementedException();
            int[,] cipher = createMatrix(cipher3, 3);
            int[,] pt = createMatrix(plain3, 3);
            int[,] pti = plainInverse(pt);
            int[,] key = multiply(cipher, pti);
            List<int> k = new List<int>();
            for (int i = 0; i < key.GetLength(0); i++)
            {
                for (int j = 0; j < key.GetLength(1); j++)
                {
                    key[i, j] = key[i, j] % 26;
                    k.Add(key[i, j]);
                }
            }
            return k;
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }



    }
}