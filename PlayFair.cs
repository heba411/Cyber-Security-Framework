using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            string plaintext = "";
            key = new string(key.Distinct().ToArray()).ToLower();
            cipherText = cipherText.ToLower();
            char[,] matrix = new char[5, 5];
            string alphabet = "abcdefghiklmnopqrstuvwxyz";
            foreach (char letter in key)
            {
                alphabet = alphabet.Replace(letter.ToString(), "");
            }

            int index = 0;
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    if (index < key.Length)
                    {
                        matrix[i, j] = key[index];
                        index++;
                    }
                    else
                    {
                        matrix[i, j] = alphabet[0];
                        alphabet = alphabet.Substring(1);
                    }
                }
            }

            StringBuilder text = new StringBuilder(cipherText);

            int row1 = 0, row2 = 0, col1 = 0, col2 = 0;
            for (int i = 0; i < text.Length; i += 2)
            {
                for (int row = 0; row < 5; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        if (matrix[row, col] == text[i])
                        {
                            row1 = row;
                            col1 = col;
                        }
                        else if (matrix[row, col] == text[i + 1])
                        {
                            row2 = row;
                            col2 = col;

                        }
                    }
                }

                if (row1 == row2)
                {
                    text[i] = matrix[row1, (col1 - 1 + 5) % 5];
                    text[i + 1] = matrix[row2, (col2 - 1 + 5) % 5];
                }
                else if (col1 == col2)
                {
                    text[i] = matrix[(row1 - 1 + 5) % 5, col1];
                    text[i + 1] = matrix[(row2 - 1 + 5) % 5, col2];
                }
                else
                {
                    text[i] = matrix[row1, col2];
                    text[i + 1] = matrix[row2, col1];
                }

            }


            for (int i = text.Length - 1; i >= 0; i--)
            {
                if (text[i] == 'x' && i % 2 != 0)
                {
                    if (i == text.Length - 1 || text[i - 1] == text[i + 1])
                    {
                        text = text.Remove(i, 1);
                    }
                }
            }

            plaintext = text.ToString();

            return plaintext.ToLower();
        }

        public string Encrypt(string plainText, string key)
        {
            string ciphertext = "";
            key = new string(key.Distinct().ToArray()).ToLower();
            plainText = plainText.ToLower();
            char[,] matrix = new char[5, 5];
            string alphabet = "abcdefghiklmnopqrstuvwxyz";
            foreach (char letter in key)
            {
                alphabet = alphabet.Replace(letter.ToString(), "");
            }

            int index = 0;
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    if (index < key.Length)
                    {
                        matrix[i, j] = key[index];
                        index++;
                    }
                    else
                    {
                        matrix[i, j] = alphabet[0];
                        alphabet = alphabet.Substring(1);
                    }
                }
            }

            StringBuilder text = new StringBuilder(plainText);
            for (int i = 0; i < text.Length - 1; i += 2)
            {
                if (text[i] == text[i + 1])
                    text.Insert(i + 1, 'x');
            }

            if (text.Length % 2 != 0)
            {
                text.Append('x');
            }


            int row1 = 0, row2 = 0, col1 = 0, col2 = 0;
            for (int i = 0; i < text.Length; i += 2)
            {
                for (int row = 0; row < 5; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        if (matrix[row, col] == text[i])
                        {
                            row1 = row;
                            col1 = col;
                        }
                        else if (matrix[row, col] == text[i + 1])
                        {
                            row2 = row;
                            col2 = col;

                        }
                    }
                }

                if (row1 == row2)
                {
                    text[i] = matrix[row1, (col1 + 1) % 5];
                    text[i + 1] = matrix[row2, (col2 + 1) % 5];
                }
                else if (col1 == col2)
                {
                    text[i] = matrix[(row1 + 1) % 5, col1];
                    text[i + 1] = matrix[(row2 + 1) % 5, col2];
                }
                else
                {
                    text[i] = matrix[row1, col2];
                    text[i + 1] = matrix[row2, col1];
                }

            }
            ciphertext = text.ToString();
            return ciphertext.ToUpper();
        }
    }
}