using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            List<int> findkey = new List<int>();

            if (plainText == cipherText)
            {
                findkey.Add(1);
                return findkey;
            }

            int Rows = 1;
            int Count = plainText.Length;
                Dictionary<int, string> dic = new Dictionary<int, string>();
                foreach (int i in Enumerable.Range(2, plainText.Length - 2))
                {
                
                int rows = (int)Math.Ceiling(Convert.ToDouble(cipherText.Length) / i);
                int cols = i;
                if (cols * rows != cipherText.Length)
                    {
                        continue;
                    }

                    List<List<char>> list = new List<List<char>>();
                    int Ccount = 0;
                    foreach (int row in Enumerable.Range(0, rows))
                    {
                        list.Add(new List<char>());
                        int count = 0;
                        while (count < cols)
                        {
                            if (Ccount == plainText.Length)
                            {
                                list[row].Add('x');
                            }
                            else
                            {
                                list[row].Add(plainText[Ccount]);
                            Ccount++;
                            }
                            count++;
                        }
                    }

                    foreach (int col in Enumerable.Range(0, cols))
                    {
                        string str = "";
                        foreach (int row in Enumerable.Range(0, rows))
                        {
                        str += list[row][col];
                        }
                        dic[col] = str;
                    }

                    bool flage = true, foundLength = true;
                    foreach (int col in Enumerable.Range(0, cols))
                    {
                        string str = dic[col];
                        foreach (int j in Enumerable.Range(0, cipherText.Length).Where(j => j % rows == 0))
                        {
                            flage = true;
                            if (cipherText[j] == str[0])
                            {
                                foreach (int row in Enumerable.Range(0, rows))
                                {
                                    if (!(cipherText[j + row] == str[row]))
                                        flage = false;
                                }
                                if (flage)
                                    break;
                            }
                            else
                                flage = false;
                        }
                        if (!flage)
                        {
                            foundLength = false;
                            break;
                        }
                    }

                    if (!foundLength)
                    {
                    dic.Clear();
                        continue;
                    }
                Rows = rows;
                Count = i;
                    break;
                }

            foreach (int i in Enumerable.Range(0, Count))
            {
                findkey.Add(0);

            }
                if (dic.Count > 0)
                {
                    foreach (int c in Enumerable.Range(0, Count))
                    {
                        double index = cipherText.IndexOf(dic[c]);
                        findkey[c] = (int)(Math.Ceiling(index / Convert.ToDouble(Rows)) + 1);
                    }
                }
            
            return findkey;

        }

        public string Decrypt(string cipherText, List<int> key)
        {
            for (int i = 0; i < cipherText.Length % key.Count; i++)
            { cipherText += 'x'; }

            cipherText = cipherText.ToLower();
            
            int cols = key.Count;
            int rows = (int)Math.Ceiling(Convert.ToDouble(cipherText.Length) / key.Count);
            string plainText = "";
            int count = 0;
            Dictionary<int, string> dic = new Dictionary<int, string>();
            for (int col = 0; col < cols; col++)
            {
                string str = "";
                int row = 0;
                while (row < rows)
                {
                    str += cipherText[count];
                    count++;
                    row++;
                }
                dic[col + 1] = str;
            }
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    plainText += dic[key[col]][row];
                }
            }

            return plainText;
           
        }

        public string Encrypt(string plainText, List<int> key)
        {
            
            
            int rows = (int)Math.Ceiling(Convert.ToDouble(plainText.Length) / key.Count);
            int cols = key.Count;
            string cipherText = "";
            List<List<char>> list = new List<List<char>>();
            int count = 0;
            for (int row = 0; row < rows; row++)
            {
                list.Add(new List<char>());
                int col = 0;
                while (col < cols)
                {
                    if (count == plainText.Length)
                    {
                        list[row].Add('x');
                    }
                    else
                    {
                        list[row].Add(plainText[count]);
                        count++;
                    }
                    col++;
                }
            }
            Dictionary<int, string> dic = new Dictionary<int, string>();
            for (int col = 0; col < cols; col++)
            {
                string str = "";
                for (int row = 0; row < rows; row++)
                {
                    str += list[row][col];
                }
                dic[key[col]] = str;
            }

            for (int col = 1; col <= cols; col++)
            {
                cipherText += dic[col];
            }

            return cipherText;

        }
    }
}