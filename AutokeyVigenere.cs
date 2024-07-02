using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public char[,] create_table()
        {
            char[,] alphabet_table = new char[26, 26];
            for (int i = 0; i < 26; ++i)
            {
                for (int j = 0; j < 26; ++j)
                {
                    alphabet_table[i, j] = (char)('A' + (i + j) % 26);
                }
            }
            return alphabet_table;
        }

        public string Analyse(string plainText, string cipherText)
        {

            //throw new NotImplementedException();
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            string key = "";
            char[,] alphabet_table = new char[26, 26];
            for (int i = 0; i < 26; ++i)
            {
                for (int j = 0; j < 26; ++j)
                {
                    alphabet_table[i, j] = (char)('A' + (i + j) % 26);
                }
            }

            for (int i = 0; i < plainText.Length; ++i)
            {
                char c = char.ToUpper(cipherText[i]);
                char p = char.ToUpper(plainText[i]);

                int rowIndex = p - 'A';

                for (int j = 0; j < 26; ++j)
                {
                    if (alphabet_table[rowIndex, j] == c)
                    {
                        int colIndex = j;
                        char k = (char)(colIndex + 'A');

                        key += k;

                    }
                }
            }
            key = key.ToLower();
            string seq = plainText[0].ToString() + plainText[1].ToString();
            int index = key.IndexOf(seq);
            key = key.Substring(0, index);

            return key;
        }

        public string Decrypt(string cipherText, string key)
        {

            //throw new NotImplementedException();
            string plainText = "";
            char[] keyStream = new char[cipherText.Length];
            char[,] alphabet_table = create_table();

            for (int i = 0; i < cipherText.Length; i++)
            {
                if (i < key.Length)
                {
                    keyStream[i] = key[i];
                }
                else
                {
                    char c = char.ToUpper(cipherText[i - key.Length]);
                    char k = char.ToUpper(keyStream[i - key.Length]);

                    int colIndex = k - 'A';

                    char p;
                    for (int j = 0; j < 26; ++j)
                    {
                        if (alphabet_table[j, colIndex] == c)
                        {
                            int rowIndex = j;
                            p = (char)(rowIndex + 'A');
                            keyStream[i] = p;

                        }
                    }

                }
            }

            for (int i = 0; i < cipherText.Length; i++)
            {
                char c = char.ToUpper(cipherText[i]);
                char k = char.ToUpper(keyStream[i]);

                int colIndex = k - 'A';

                for (int j = 0; j < 26; ++j)
                {
                    if (alphabet_table[j, colIndex] == c)
                    {
                        int rowIndex = j;
                        char p = (char)(rowIndex + 'A');
                        plainText += p;
                    }
                }
            }
            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            string cipherText = "";
            plainText = plainText.ToLower();
            key = key.ToLower();
            char[] keyStream = new char[plainText.Length];
            for (int i = 0; i < plainText.Length; i++)
            {
                if (i < key.Length)
                {
                    keyStream[i] = key[i];
                }
                else
                {
                    keyStream[i] = plainText[i - key.Length];
                }
            }

            char[,] alphabet_table = create_table();
            char cipher;
            for (int i = 0; i < plainText.Length; ++i)
            {
                char p = char.ToUpper(plainText[i]);
                char k = char.ToUpper(keyStream[i]);

                int rowIndex = p - 'A';
                int columnIndex = k - 'A';

                cipher = alphabet_table[rowIndex, columnIndex];
                cipherText += cipher;

            }

            return cipherText;
        }
    }
}