using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary
{

    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            //throw new NotImplementedException();
            string encryText = "";
            foreach (char letter in plainText)
            {
                if (char.IsLetter(letter))
                {
                    char start = char.IsUpper(letter) ? 'A' : 'a';
                    encryText += (char)(((letter - start + key) % 26 + 26) % 26 + start);
                }
                else
                {
                    encryText += letter;
                }
            }

            return encryText;
        }

        public string Decrypt(string cipherText, int key)
        {
            //throw new NotImplementedException();
            return Encrypt(cipherText, 26 - key);
        }

        public int Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            int match = 0;
            int answer = 0;
            for (int key = 0; key < 26; key++)
            {
                string decryText = Decrypt(cipherText, key).ToLower();
                 int count = 0;

                for (int i = 0; i < Math.Min(plainText.Length, decryText.Length); i++)
                {
                    if (plainText[i].ToString().ToLower() == decryText[i].ToString())
                    {
                        count++;
                    }
                }
                if (count > match)
                {
                    match = count;
                    answer = key;
                }
            }

            return answer;
        }
    }
}
