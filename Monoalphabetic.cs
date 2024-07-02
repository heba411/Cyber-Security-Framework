using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            HashSet<char> keys = new HashSet<char>();

            char x = 'a'; int sz = cipherText.Length; int num = 0;
            char[] charArray = new char[26]; // answer but in array to be able to change it

            for (int i = 0; i < sz; i++)
            {
                num = plainText[i] - x;// order of letter 'a' - 'a' = 0
                charArray[num] = cipherText[i]; // set answer to be like cipher letter
                keys.Add(cipherText[i]);//add to hash set to make sure we don't choose it again
            }
            for (int i = 0; i < 26; i++)
            {
                if (charArray[i] == '\0')//empty answer letter not set 
                {
                    while (keys.Contains(x)) //choose letter not in the hashSet
                    {
                        x++;
                    }
                    charArray[i] = x;
                    x++;//add bec it is already taken
                }
            }
            string answer = new string(charArray);
            return answer;

        }

        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();
            string result = "";
            string cipher = cipherText.ToLower();
            char[] word = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
                'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
                's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] key_word = key.ToCharArray();

            for (int i = 0; i < cipher.Length; i++)
            {
                for (int j = 0; j < 26; ++j)
                {
                    if (cipher[i] == key_word[j])
                    {
                        result += word[j];
                        break;
                    }
                }

            }
            return result;

        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            string result = "";
            char[] word = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
                    'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
                    's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] key_word = key.ToCharArray();

            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < 26; ++j)
                {
                    if (plainText[i] == word[j])
                    {
                        result += key_word[j];
                        break;
                    }
                }

            }
            return result.ToUpper();
        }







        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	=
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        /// 

        public string AnalyseUsingCharFrequency(string cipher)
        {

            //throw new NotImplementedException();
            char[] charFrq = new char[] {'E', 'T', 'A', 'O','I','N','S','R','H','L','D','C',
                                        'U','M','F','P','G','W','Y','B','V','K','X','J','Q','Z'};
            Dictionary<char, int> letterFreq = new Dictionary<char, int>();
            string pt = "";
            for (int i = 0; i < cipher.Length; i++)
            {
                char l = cipher[i];
                if (!letterFreq.ContainsKey(l))
                {
                    letterFreq.Add(l, 1);
                }
                else
                {
                    letterFreq[l]++;
                }
            }
            var sortedFreq = from x in letterFreq orderby x.Value descending select x;
            Dictionary<char, char> charMap = new Dictionary<char, char>();
            for (int i = 0; i < charFrq.Length; i++)
            {
                charMap.Add(sortedFreq.ElementAt(i).Key, charFrq[i]);
                //Console.WriteLine("Key: {0}, Value: {1}",charMap.ElementAt(i).Key, charMap.ElementAt(i).Value);
            }
            for (int i = 0; i < cipher.Length; i++)
            {
                pt += charMap[cipher[i]];
            }
            //Dictionary<char, float> freqPercent = new Dictionary<char, float>();
            foreach (KeyValuePair<char, int> x in sortedFreq)
            {
                Console.WriteLine("Key: {0}, Value: {1}", x.Key, x.Value);
                //float per = ((float)x.Value / (float)cipher.Length) * 100;
                //Console.WriteLine(per);
                //freqPercent.Add(x.Key, per);
            }
            return pt.ToLower();




        }
    }
}