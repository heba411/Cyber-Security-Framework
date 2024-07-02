using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
       
       static string[,] AES_S_BOX = {
            { "63", "7c", "77", "7b", "f2", "6b", "6f", "c5", "30", "01", "67", "2b", "fe", "d7", "ab", "76" },
            { "ca", "82", "c9", "7d", "fa", "59", "47", "f0", "ad", "d4", "a2", "af", "9c", "a4", "72", "c0" },
            { "b7", "fd", "93", "26", "36", "3f", "f7", "cc", "34", "a5", "e5", "f1", "71", "d8", "31", "15" },
            { "04", "c7", "23", "c3", "18", "96", "05", "9a", "07", "12", "80", "e2", "eb", "27", "b2", "75" },
            { "09", "83", "2c", "1a", "1b", "6e", "5a", "a0", "52", "3b", "d6", "b3", "29", "e3", "2f", "84" },
            { "53", "d1", "00", "ed", "20", "fc", "b1", "5b", "6a", "cb", "be", "39", "4a", "4c", "58", "cf" },
            { "d0", "ef", "aa", "fb", "43", "4d", "33", "85", "45", "f9", "02", "7f", "50", "3c", "9f", "a8" },
            { "51", "a3", "40", "8f", "92", "9d", "38", "f5", "bc", "b6", "da", "21", "10", "ff", "f3", "d2" },
            { "cd", "0c", "13", "ec", "5f", "97", "44", "17", "c4", "a7", "7e", "3d", "64", "5d", "19", "73" },
            { "60", "81", "4f", "dc", "22", "2a", "90", "88", "46", "ee", "b8", "14", "de", "5e", "0b", "db" },
            { "e0", "32", "3a", "0a", "49", "06", "24", "5c", "c2", "d3", "ac", "62", "91", "95", "e4", "79" },
            { "e7", "c8", "37", "6d", "8d", "d5", "4e", "a9", "6c", "56", "f4", "ea", "65", "7a", "ae", "08" },
            { "ba", "78", "25", "2e", "1c", "a6", "b4", "c6", "e8", "dd", "74", "1f", "4b", "bd", "8b", "8a" },
            { "70", "3e", "b5", "66", "48", "03", "f6", "0e", "61", "35", "57", "b9", "86", "c1", "1d", "9e" },
            { "e1", "f8", "98", "11", "69", "d9", "8e", "94", "9b", "1e", "87", "e9", "ce", "55", "28", "df" },
            { "8c", "a1", "89", "0d", "bf", "e6", "42", "68", "41", "99", "2d", "0f", "b0", "54", "bb", "16" }
        };
        static string[,] AES_S_BOX_Dec = {
              {"52", "09", "6a", "d5", "30", "36", "a5", "38", "bf", "40", "a3", "9e", "81", "f3", "d7", "fb"},
              {"7c", "e3", "39", "82", "9b", "2f", "ff", "87", "34", "8e", "43", "44", "c4", "de", "e9", "cb"},
              {"54", "7b", "94", "32", "a6", "c2", "23", "3d", "ee", "4c", "95", "0b", "42", "fa", "c3", "4e"},
              {"08", "2e", "a1", "66", "28", "d9", "24", "b2", "76", "5b", "a2", "49", "6d", "8b", "d1", "25"},
              {"72", "f8", "f6", "64", "86", "68", "98", "16", "d4", "a4", "5c", "cc", "5d", "65", "b6", "92"},
              {"6c", "70", "48", "50", "fd", "ed", "b9", "da", "5e", "15", "46", "57", "a7", "8d", "9d", "84"},
              {"90", "d8", "ab", "00", "8c", "bc", "d3", "0a", "f7", "e4", "58", "05", "b8", "b3", "45", "06"},
              {"d0", "2c", "1e", "8f", "ca", "3f", "0f", "02", "c1", "af", "bd", "03", "01", "13", "8a", "6b"},
              {"3a", "91", "11", "41", "4f", "67", "dc", "ea", "97", "f2", "cf", "ce", "f0", "b4", "e6", "73"},
              {"96", "ac", "74", "22", "e7", "ad", "35", "85", "e2", "f9", "37", "e8", "1c", "75", "df", "6e"},
              {"47", "f1", "1a", "71", "1d", "29", "c5", "89", "6f", "b7", "62", "0e", "aa", "18", "be", "1b"},
              {"fc", "56", "3e", "4b", "c6", "d2", "79", "20", "9a", "db", "c0", "fe", "78", "cd", "5a", "f4"},
              {"1f", "dd", "a8", "33", "88", "07", "c7", "31", "b1", "12", "10", "59", "27", "80", "ec", "5f"},
              {"60", "51", "7f", "a9", "19", "b5", "4a", "0d", "2d", "e5", "7a", "9f", "93", "c9", "9c", "ef"},
              {"a0", "e0", "3b", "4d", "ae", "2a", "f5", "b0", "c8", "eb", "bb", "3c", "83", "53", "99", "61"},
              {"17", "2b", "04", "7e", "ba", "77", "d6", "26", "e1", "69", "14", "63", "55", "21", "0c", "7d"}
        };
       
        /// key 
        public static string[,] createStringKeyMatrix(List<string> key)
        {
            int size = (int)Math.Sqrt(key.Count);
            string[,] matrix = new string[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[j, i] = key[i * size + j];
                }
            }
           
            return matrix;
        }
        public static int check(char c)
        {
            int r;
            if (c == 'A' || c == 'a')
                r = 10;
            else if (c == 'B' || c == 'b')
                r = 11;
            else if (c == 'C' || c == 'c')
                r = 12;
            else if (c == 'D' || c == 'd')
                r = 13;
            else if (c == 'E' || c == 'e')
                r = 14;
            else if (c == 'F' || c == 'f')
                r = 15;
            else
                r = c - '0';
            return r;
        }
        public static List<string> XORround1(List<string> s1, List<string> s2, List<string> s3)
        {
            List<string> list1 = XOR(s1, s2);
            List<string> finalList = XOR(list1, s3);
            return finalList;
        }
        public static List<string> XOR(List<string> s1, List<string> s2)
        {
            List<string> finalList = new List<string>();
            for (int i = 0; i < 4; i++)
            {

                string s = s1[i].Substring(0, 1);
                string ss = s1[i].Substring(1, 1);
                int h1 = Convert.ToInt32(s, 16);
                int h2 = Convert.ToInt32(ss, 16);
                string j = s2[i].Substring(0, 1);
                string jj = s2[i].Substring(1, 1);
                int h3 = Convert.ToInt32(j, 16);
                int h4 = Convert.ToInt32(jj, 16);
                int result1 = h1 ^ h3;
                int result2 = h2 ^ h4;
                string result1Hex = result1.ToString("X");
                string result2Hex = result2.ToString("X");
                if (result1Hex == "0")
                {
                    finalList.Add("0" + result2Hex);
                }
                else if (result2Hex == "0")
                {
                    finalList.Add(result1Hex + "0");
                }
                else if (result1Hex == "0" && result2Hex == "0")
                {
                    finalList.Add("00");
                }
                else
                {
                    finalList.Add(result1Hex + result2Hex);
                }
            }
            return finalList;
        }
        public static List<string> getRCON_Round(int roundNumber)
        {
            string[,] RCON =
            {
         { "01","02","04","08","10","20","40","80","1b","36"},
         { "00","00","00","00","00","00","00","00","00","00"},
         { "00","00","00","00","00","00","00","00","00","00"},
         { "00","00","00","00","00","00","00","00","00","00"}
     };
            List<string> RCON_List = new List<string>();
            int row = RCON.GetLength(0);
            int col = RCON.GetLength(1);
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    RCON_List.Add(RCON[j, i]);
                }
            }
            List<string> round1 = RCON_List.Take(4).ToList();
            List<string> round2 = RCON_List.Skip(4).Take(4).ToList();
            List<string> round3 = RCON_List.Skip(8).Take(4).ToList();
            List<string> round4 = RCON_List.Skip(12).Take(4).ToList();
            List<string> round5 = RCON_List.Skip(16).Take(4).ToList();
            List<string> round6 = RCON_List.Skip(20).Take(4).ToList();
            List<string> round7 = RCON_List.Skip(24).Take(4).ToList();
            List<string> round8 = RCON_List.Skip(28).Take(4).ToList();
            List<string> round9 = RCON_List.Skip(32).Take(4).ToList();
            List<string> round10 = RCON_List.Skip(36).Take(4).ToList();
            if (roundNumber == 1)
            {
                return round1;
            }
            else if (roundNumber == 2)
            {
                return round2;
            }
            else if (roundNumber == 3)
            {
                return round3;
            }
            else if (roundNumber == 4)
            {
                return round4;
            }
            else if (roundNumber == 5)
            {
                return round5;
            }
            else if (roundNumber == 6)
            {
                return round6;
            }
            else if (roundNumber == 7)
            {
                return round7;
            }
            else if (roundNumber == 8)
            {
                return round8;
            }
            else if (roundNumber == 9)
            {
                return round9;
            }
            return round10;

        }
        public static List<string> RoundkeySchedule(List<string> keyList, int roundNumber)
        {
            
            //createStringKeyMatrix(keyList);
            List<string> firstcolumn = keyList.Take(4).ToList();
            List<string> secondcolumn = keyList.Skip(4).Take(4).ToList();
            List<string> thirdcolumn = keyList.Skip(8).Take(4).ToList();
            List<string> lastcolumn = keyList.Skip(12).Take(4).ToList();

            List<string> lastcolumne = keyList.Skip(12).Take(4).ToList();

            // Rotate
            string firstElement = lastcolumne[0];
            lastcolumne.RemoveAt(0);
            lastcolumne.Insert(lastcolumne.Count, firstElement);

            //subtype
            List<string> last = new List<string>();

            foreach (var item in lastcolumne)
            {
                char i = item[0];
                //Console.WriteLine(i);
                int ii = check(i);
                //Console.WriteLine(ii);
                char j = item[1];
                //Console.WriteLine(i);
                int jj = check(j);
                //Console.WriteLine(jj);
                string s = AES_S_BOX[ii, jj];
                last.Insert(0, s);
            }
            last.Reverse();

            //XOR
            List<string> RCON_Round = getRCON_Round(roundNumber);
            List<string> finalColumn1 = XORround1(last, firstcolumn, RCON_Round);
            List<string> finalColumn2 = XOR(finalColumn1, secondcolumn);
            List<string> finalColumn3 = XOR(finalColumn2, thirdcolumn);
            List<string> finalColumn4 = XOR(finalColumn3, lastcolumn);
            List<string> finalKeyList = finalColumn1.Concat(finalColumn2).Concat(finalColumn3).Concat(finalColumn4).ToList();

            return finalKeyList;
        }
        public static List<string[,]> KeySchedule(string key)
        {
            List<string[,]> allKeys = new List<string[,]>();
            key = key.Substring(2);
            Console.WriteLine(key);
            List<string> keyList = Enumerable.Range(0, key.Length / 2).Select(i => key.Substring(i * 2, 2)).ToList();
            for (int i = 1; i <= 10; i++)
            {

                List<string> finalKeyList = RoundkeySchedule(keyList, i);
                string[,] roundKey = createStringKeyMatrix(finalKeyList);
                allKeys.Add(roundKey);
                keyList = finalKeyList;
            }
            return allKeys;
        }
        ///// end key

        // mix col
        static string[,] mix_columns(string[,] mix, string[,] arr)
        {
            const string _1b = "00011011";
            string bi_arr;
            int shifts;
            int odd;
            char xor_1b;
            string shifted = "";
            string[,] result = new string[4, 4];
            string sum;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sum = "00000000";
                    for (int l = 0; l < 4; ++l)
                    {
                        bi_arr = Convert.ToString(Convert.ToInt32(arr[l, j], 16), 2).PadLeft(arr[l, j].Length * 4, '0');
                        shifts = Convert.ToInt32(mix[i, l], 16) / 2;
                        odd = Convert.ToInt32(mix[i, l], 16) % 2;
                        xor_1b = bi_arr[0];

                        if (shifts == 0)
                        {
                            shifted = bi_arr;
                        }
                        else
                        {
                            shifted = bi_arr.Substring(1) + '0';
                            for (int k = 1; k < shifts; k++)
                            {
                                shifted = bi_arr.Substring(1) + '0';
                            }
                        }

                        if (xor_1b == '1' && shifts != 0)          //shifted xor 1b
                        {
                            shifted = xor(_1b, shifted).ToString();
                        }

                        if (odd == 1 && shifts != 0)
                        {
                            shifted = xor(bi_arr, shifted).ToString();
                        }

                        sum = xor(shifted, sum).ToString();
                    }

                    int int_sum = Convert.ToInt32(sum, 2);
                    result[i, j] = int_sum.ToString("X2");
                }
            }

            StringBuilder xor(string binary1, string binary2)
            {
                StringBuilder res = new StringBuilder();
                for (int k = 0; k <= 7; k++)
                {
                    char bit1 = binary1[k];
                    char bit2 = binary2[k];
                    if (bit1 == bit2)
                    {
                        res.Append('0');
                    }
                    else
                    {
                        res.Append('1');
                    }
                }
                return res;
            }

            return result;
        }
        // end mix
        static string[,] mix_columns_dec(string[,] mix, string[,] arr)
        {
            string[,] result = new string[4, 4];

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    int sum = 0;

                    for (int k = 0; k < 4; k++)
                    {
                        int mix_int = Convert.ToInt32(mix[i, k], 16);
                        int arr_int = Convert.ToInt32(arr[k, j], 16);

                        for (int b = 0; b < 8; b++)
                        {
                            if ((mix_int & (1 << b)) != 0)
                            {
                                sum ^= arr_int;
                            }
                            arr_int <<= 1;
                            if ((arr_int & 0x100) != 0)
                            {
                                arr_int ^= 0x11B;
                            }
                        }
                    }

                    result[i, j] = sum.ToString("X2");
                }
            }

            return result;
        }
        // inv shift
        public string[,] inverseShiftRow(string[,] shiftedArrs)
        {
            string[,] original = new string[4, 4];
            int temp = 0;
            for (int i = 0; i < 4; i++)
            {
                int cnt = 0;
                for (int j = 4 - i; j < 4; j++)
                {
                    if (temp == 0)
                    {
                        original[i, j] = shiftedArrs[i, j];
                    }
                    else
                    {
                        original[i, cnt] = shiftedArrs[i, j];
                        cnt += 1;
                        if (temp == cnt)
                        {
                            break;
                        }
                    }
                }
                temp++;//no of shifted elements
                int k = 0;
                for (int j = cnt; j < 4; j++)
                {
                    if (temp == 0)
                    {
                        break;
                    }
                    original[i, j] = shiftedArrs[i, k];
                    k += 1;
                }
            }
            return original;
        }
        // end inv shift

        // shift
        string[,] shiftRows(string[,] input)
        {
            string[,] shiftedArrs = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                int cnt = 0;// number of elements that won't be shifted
                int temp = 0;//column counter of input
                for (int j = i; j < 4; j++)
                {
                    shiftedArrs[i, cnt] = input[i, j];
                    cnt++;
                }
                for (int j = cnt; j < 4; j++)
                {
                    if (cnt == 4) break;
                    shiftedArrs[i, j] = input[i, temp];
                    temp = temp + 1;
                    Console.WriteLine(shiftedArrs[i, j]);
                }
                Console.WriteLine();
            }

            return shiftedArrs;
        }

        static string[,] add_round_key(string[,] state, string[,] key)
        {
            string[,] result = new string[4, 4];
            string state_bi;
            string key_bi;
            string res;
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    state_bi = Convert.ToString(Convert.ToInt32(state[i, j], 16), 2).PadLeft(8, '0');
                    key_bi = Convert.ToString(Convert.ToInt32(key[i, j], 16), 2).PadLeft(8, '0');
                    res = xor(state_bi, key_bi).ToString();
                    result[i, j] = Convert.ToInt32(res, 2).ToString("X").PadLeft(2, '0');

                }
            }

            StringBuilder xor(string binary1, string binary2)
            {
                StringBuilder res0 = new StringBuilder();
                for (int k = 0; k <= 7; k++)
                {
                    char bit1 = binary1[k];
                    char bit2 = binary2[k];
                    if (bit1 == bit2)
                    {
                        res0.Append('0');
                    }
                    else
                    {
                        res0.Append('1');
                    }
                }
                return res0;
            }

            return result;
        }
      
        public static int mapToIndex(char c)
        {
            switch (c)
            {
                case 'a':
                    return 10;
                case 'b':
                    return 11;
                case 'c':
                    return 12;
                case 'd':
                    return 13;
                case 'e':
                    return 14;
                case 'f':
                    return 15;
                default:
                    return c - '0';
            }
        }
        public static string[,] subBytesEncrypt(string text)
        {
            text = text.ToLower();
            Console.WriteLine(text);
            string[] s = new string[16];
            int x, y;
            int c = 0;
            for (int i = 2; i < text.Length - 1; i += 2)
            {
                x = mapToIndex(text[i]);
                y = mapToIndex(text[i + 1]);
                s[c] = AES_S_BOX[x, y];
                c++;
            }
            string[,] r = new string[4, 4];
            c = 0;
            for (int j = 0; j < r.GetLength(1); ++j)
            {
                for (int i = 0; i < r.GetLength(0); ++i)
                {
                    r[i, j] = s[c];
                    c++;
                }
            }
            return r;
        }
        public static string[,] subBytesDecrypt(string text)
        {
            text = text.ToLower();
            Console.WriteLine(text);
            string[] s = new string[16];
            int x, y;
            int c = 0;
            for (int i = 2; i < text.Length - 1; i += 2)
            {
                x = mapToIndex(text[i]);
                y = mapToIndex(text[i + 1]);
                s[c] = AES_S_BOX_Dec[x, y];
                c++;
            }
            string[,] r = new string[4, 4];
            c = 0;
            for (int j = 0; j < r.GetLength(1); ++j)
            {
                for (int i = 0; i < r.GetLength(0); ++i)
                {
                    r[i, j] = s[c];
                    c++;
                }
            }
            return r;
        }
       

        public string[,] Str_to_arr(string input)
        {
            string[,] arr = new string[4, 4];
            int x0 = 2;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string str = "";
                    str = String.Concat(input[x0], input[x0 + 1]);
                    arr[j, i] = str;
                    x0 += 2;
                }
            }
            return arr;
        }
       
        public string Arr_to_str(string[,] input)
        {
            string str = "0x";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    str = String.Concat(str, input[j, i]);
                }
            }
            return str;
        }
        public override string Decrypt(string cipherText, string key)
        {
        
            string[,] Mix_Dec = {
                {"0E","0B","0D","09"},
                {"09","0E","0B","0D"},
                { "0D","09","0E","0B"},
                { "0B","0D","09","0E"}
            };

            string Dec = "";
            List<string[,]> allKeys = new List<string[,]>();
            allKeys = KeySchedule(key);
            string[,] plain = add_round_key(Str_to_arr(cipherText), allKeys[9]);
            plain = inverseShiftRow(plain);
            plain = subBytesDecrypt(Arr_to_str( plain ));
            for (int i = 9; i > 0; i--)
            {
                plain = add_round_key(plain, allKeys[i-1]);
                plain = mix_columns_dec(Mix_Dec,plain);
                plain = inverseShiftRow(plain);
                plain = subBytesDecrypt(Arr_to_str(plain));
            }
            plain = add_round_key(plain, Str_to_arr(key));
            Dec = Arr_to_str(plain);
            return Dec;
        }
        


        public override string Encrypt(string plainText, string key)
        {
            string[,] Mix =
         {
            {"02", "03", "01", "01"},
            {"01", "02", "03", "01"},
            {"01", "01", "02", "03"},
            {"03", "01", "01", "02"}
        };
             
          
            string Enc = "";   
            string[,] cipher = add_round_key(Str_to_arr(plainText), Str_to_arr(key));
            List<string[,]> allKeys = new List<string[,]>();
            allKeys = KeySchedule(key);
            for (int i = 0; i < 9; i++)
            {
               
                cipher = subBytesEncrypt(Arr_to_str( cipher));
                cipher = shiftRows(cipher);
                cipher = mix_columns(Mix,cipher);
                cipher = add_round_key(cipher, allKeys[i]);
            }
           
            cipher = subBytesEncrypt(Arr_to_str( cipher));
            cipher = shiftRows(cipher);
            cipher = add_round_key(cipher, allKeys[9]);
            Enc = Arr_to_str(cipher);
            return Enc.ToUpper();

        }
    }
}
