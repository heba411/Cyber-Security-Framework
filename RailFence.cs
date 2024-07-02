using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            int len = 0;
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            for (int key = 1; key < plainText.Length; key++)
            {
                string encrypted = Encrypt(plainText, key);
                if (encrypted == cipherText)
                {
                    len = key;
                }
                else if (encrypted.Length < cipherText.Length) len = key;
            }
            return len;
        }

        public string Decrypt(string cipherText, int key)
        {
            int cnt = 0, sz = cipherText.Length;
            int len = sz / key;
            if (len * key < sz) len++;
            char[,] arr = new char[key, len];
            string answer = "";
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (cnt == sz) break;
                    arr[i, j] = cipherText[cnt];
                    cnt++;
                }
                if (cnt == sz) break;
            }
            for (int j = 0; j < len; j++)
            {
                for (int i = 0; i < key; i++)
                {
                    if (arr[i, j] != '\0')
                        answer += arr[i, j];
                }
            }
            return answer;
        }

        public string Encrypt(string plainText, int key)
        {
            string answer = ""; int ind = 0;

            while (ind < key)
            {
                for (int i = ind; i < plainText.Length; i += key)
                {
                    answer += plainText[i];
                }
                ind++;
            }
            return answer;
        }
    }
}
