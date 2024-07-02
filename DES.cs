using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class DES : CryptographicTechnique
    {
        // DES ///////////////////////////////////////////////////////////////////////////
        public static Dictionary<int, char> hex_to_bin(string key)
        {
            string binKey = "";
            Dictionary<int, char> dict = new Dictionary<int, char>();
            int counter = 1;
            key = key.Substring(2);
            foreach (char c in key)
            {
                string binaryDigit = Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0');
                binKey += binaryDigit;
            }
            for (int i = 0; i < binKey.Length; i++)
            {
                dict.Add(counter, binKey[i]);
                counter++;
            }
            return dict;
        }
        public static string permutedKeyinPC1(Dictionary<int, char> binaryKey)
        {
            string permutedKey = "";
            int[] PC1 = { 57, 49, 41, 33, 25, 17, 9, 1,
                      58, 50, 42, 34, 26, 18, 10, 2,
                      59, 51, 43, 35, 27, 19, 11, 3,
                      60, 52, 44, 36, 63, 55, 47, 39,
                      31, 23, 15, 7, 62, 54, 46, 38,
                      30, 22, 14, 6, 61, 53, 45, 37,
                      29, 21, 13, 5, 28, 20, 12, 4 };
            for (int i = 0; i < PC1.Length; i++)
            {

                char c = binaryKey[PC1[i]];
                permutedKey += c;
            }
            return permutedKey;
        }

        public static string shiftLeft(string stringToShift, int RoundNUmber)
        {
            string shifted;
            if (RoundNUmber == 1 || RoundNUmber == 2 || RoundNUmber == 9 || RoundNUmber == 16)
            {
                char firstchar = stringToShift[0];
                shifted = stringToShift.Substring(1);
                shifted += firstchar;
            }
            else
            {
                string removedChars = stringToShift.Substring(0, 2);
                shifted = stringToShift.Substring(2);
                shifted += removedChars;
            }


            return shifted;
        }
        public static string permutedKeyinPC2(string concatenatedKey)
        {
            string permutedKey = "";
            int[] PC2 = {  14, 17, 11, 24, 1, 5,
                       3, 28, 15, 6, 21, 10,
                       23, 19, 12, 4, 26, 8,
                       16, 7, 27, 20, 13, 2,
                       41, 52, 31, 37, 47, 55,
                       30, 40, 51, 45, 33, 48,
                       44, 49, 39, 56, 34, 53,
                       46, 42, 50, 36, 29, 32 };
            Dictionary<int, char> concKey = new Dictionary<int, char>();
            int counter = 1;
            for (int i = 0; i < concatenatedKey.Length; i++)
            {
                concKey.Add(counter, concatenatedKey[i]);
                counter++;
            }
            for (int i = 0; i < PC2.Length; i++)
            {

                char c = concKey[PC2[i]];
                permutedKey += c;
            }
            return permutedKey;
        }
        public static List<string> DES_keySchedule(string key)
        {
            List<string> finalKeysList = new List<string>();
            Dictionary<int, char> binaryKey = hex_to_bin(key);
            string permutedKey = permutedKeyinPC1(binaryKey);
            int len = permutedKey.Length / 2;
            string C0 = permutedKey.Substring(0, len);
            string D0 = permutedKey.Substring(28, len);
            for (int i = 1; i <= 16; i++)
            {
                string C = shiftLeft(C0, i);
                string D = shiftLeft(D0, i);
                string concatenatedKey = string.Concat(C, D);
                string finalKey = permutedKeyinPC2(concatenatedKey);
                finalKeysList.Add(finalKey);
                C0 = C;
                D0 = D;
            }
            return finalKeysList;
        }
        public static string PadTo64Bits(string input)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(input);
            while (sb.Length < 64)
            {
                sb.Append('0');
            }
            return sb.ToString();
        }
        public static string init_perm(string s)
        {
            Dictionary<int, char> binInput = hex_to_bin(s);
            int[] Init_IP = {   58, 50, 42, 34, 26, 18, 10, 2,
                            60, 52, 44, 36, 28, 20, 12, 4,
                            62, 54, 46, 38, 30, 22, 14, 6,
                            64, 56, 48, 40, 32, 24, 16, 8,
                            57, 49, 41, 33, 25, 17, 9, 1,
                            59, 51, 43, 35, 27, 19, 11, 3,
                            61, 53, 45, 37, 29, 21, 13, 5,
                            63, 55, 47, 39, 31, 23, 15, 7 };
            string ans = "";
            for (int i = 0; i < Init_IP.Length; i++)
            {
                char c = binInput[Init_IP[i]];
                ans += c;
            }
            return ans;
        }
        public static string Expantion(string input, int[] expand_table)
        {
            var table = new Dictionary<int, char>();
            for (int i = 1; i <= 32; i++)
            {
                table[i] = input[i - 1];

            }
            char[] arr = new char[48];
            for (int i = 0; i < 48; i++)
            {
                arr[i] = table[expand_table[i]];
            }
            string output = new string(arr);
            return output;
        }
        public static StringBuilder xor(string binary1, string binary2)
        {
            StringBuilder res = new StringBuilder();
            for (int k = 0; k < binary1.Length; k++)
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
        public static string sub_boxes(string data)
        {
            string[,] s1 = {
                {"14","4","13","1","2","15","11","8","3","10","6","12","5","9","0","7"},
                {"0","15","7","4","14","2","13","1","10","6","12","11","9","5","3","8"},
                {"4","1","14","8","13","6","2","11","15","12","9","7","3","10","5","0"},
                {"15","12","8","2","4","9","1","7","5","11","3","14","10","0","6","13"},
            };
            string[,] s2 = {
                {"15","1","8","14","6","11","3","4","9","7","2","13","12","0","5","10"},
                {"3","13","4","7","15","2","8","14","12","0","1","10","6","9","11","5"},
                {"0","14","7","11","10","4","13","1","5","8","12","6","9","3","2","15"},
                {"13","8","10","1","3","15","4","2","11","6","7","12","0","5","14","9"},
            };
            string[,] s3 = {
                { "10", "0", "9", "14", "6", "3", "15", "5", "1", "13", "12", "7", "11", "4", "2", "8" },
                { "13", "7", "0", "9", "3", "4", "6", "10", "2", "8", "5", "14", "12", "11", "15", "1" },
                { "13", "6", "4", "9", "8", "15", "3", "0", "11", "1", "2", "12", "5", "10", "14", "7" },
                { "1", "10", "13", "0", "6", "9", "8", "7", "4", "15", "14", "3", "11", "5", "2", "12" }
            };
            string[,] s4 = {
                { "7", "13", "14", "3", "0", "6", "9", "10", "1", "2", "8", "5", "11", "12", "4", "15" },
                { "13", "8", "11", "5", "6", "15", "0", "3", "4", "7", "2", "12", "1", "10", "14", "9" },
                { "10", "6", "9", "0", "12", "11", "7", "13", "15", "1", "3", "14", "5", "2", "8", "4" },
                { "3", "15", "0", "6", "10", "1", "13", "8", "9", "4", "5", "11", "12", "7", "2", "14" }
            };
            string[,] s5 = {
                { "2", "12", "4", "1", "7", "10", "11", "6", "8", "5", "3", "15", "13", "0", "14", "9" },
                { "14", "11", "2", "12", "4", "7", "13", "1", "5", "0", "15", "10", "3", "9", "8", "6" },
                { "4", "2", "1", "11", "10", "13", "7", "8", "15", "9", "12", "5", "6", "3", "0", "14" },
                { "11", "8", "12", "7", "1", "14", "2", "13", "6", "15", "0", "9", "10", "4", "5", "3" }
            };
            string[,] s6 = {
                { "12", "1", "10", "15", "9", "2", "6", "8", "0", "13", "3", "4", "14", "7", "5", "11" },
                { "10", "15", "4", "2", "7", "12", "9", "5", "6", "1", "13", "14", "0", "11", "3", "8" },
                { "9", "14", "15", "5", "2", "8", "12", "3", "7", "0", "4", "10", "1", "13", "11", "6" },
                { "4", "3", "2", "12", "9", "5", "15", "10", "11", "14", "1", "7", "6", "0", "8", "13" }
            };
            string[,] s7 = {
                { "4", "11", "2", "14", "15", "0", "8", "13", "3", "12", "9", "7", "5", "10", "6", "1" },
                { "13", "0", "11", "7", "4", "9", "1", "10", "14", "3", "5", "12", "2", "15", "8", "6" },
                { "1", "4", "11", "13", "12", "3", "7", "14", "10", "15", "6", "8", "0", "5", "9", "2" },
                { "6", "11", "13", "8", "1", "4", "10", "7", "9", "5", "0", "15", "14", "2", "3", "12" }
            };
            string[,] s8 = {
                { "13", "2", "8", "4", "6", "15", "11", "1", "10", "9", "3", "14", "5", "0", "12", "7" },
                { "1", "15", "13", "8", "10", "3", "7", "4", "12", "5", "6", "11", "0", "14", "9", "2" },
                { "7", "11", "4", "1", "9", "12", "14", "2", "0", "6", "10", "13", "15", "3", "5", "8" },
                { "2", "1", "14", "7", "4", "10", "8", "13", "15", "12", "9", "0", "3", "5", "6", "11" }
            };

            string[] sub_data = new string[8];
            string[] result = new string[8];
            int c = 0;
            for (int i = 0; i < data.Length; i += 6)
            {
                sub_data[c] = data.Substring(i, 6);
                c++;
            }

            for (int i = 0; i < 8; ++i)
            {
                string rownum = sub_data[i][0].ToString() + sub_data[i][5].ToString();
                string colnum = sub_data[i].Substring(1, 4);
                int row = Convert.ToInt32(rownum, 2);
                int col = Convert.ToInt32(colnum, 2);
                if (i == 0)
                {
                    string value = s1[row, col];
                    result[0] = Convert.ToString(int.Parse(value), 2).PadLeft(4, '0');
                }
                if (i == 1)
                {
                    string value = s2[row, col];
                    result[1] = Convert.ToString(int.Parse(value), 2).PadLeft(4, '0');
                }
                if (i == 2)
                {
                    string value = s3[row, col];
                    result[2] = Convert.ToString(int.Parse(value), 2).PadLeft(4, '0');
                }
                if (i == 3)
                {
                    string value = s4[row, col];
                    result[3] = Convert.ToString(int.Parse(value), 2).PadLeft(4, '0');
                }
                if (i == 4)
                {
                    string value = s5[row, col];
                    result[4] = Convert.ToString(int.Parse(value), 2).PadLeft(4, '0');
                }
                if (i == 5)
                {
                    string value = s6[row, col];
                    result[5] = Convert.ToString(int.Parse(value), 2).PadLeft(4, '0');
                }
                if (i == 6)
                {
                    string value = s7[row, col];
                    result[6] = Convert.ToString(int.Parse(value), 2).PadLeft(4, '0');
                }
                if (i == 7)
                {
                    string value = s8[row, col];
                    result[7] = Convert.ToString(int.Parse(value), 2).PadLeft(4, '0');
                }
            }

            string res = string.Join("", result);
            return res;
        }
        public static string Perm(string s)
        {

            int[] P = { 16, 7, 20, 21, 29, 12, 28, 17,
                    1, 15, 23, 26, 5, 18, 31, 10,
                    2, 8, 24, 14, 32, 27, 3, 9, 19,
                    13, 30, 6, 22, 11, 4, 25 };
            string ans = "";
            for (int i = 0; i < P.Length; ++i)
            {
                ans += s[P[i] - 1];
            }
            return ans;
        }
        public static string final_Perm(string s)
        {
            int[] permutationTable = {
            40,  8, 48, 16, 56, 24, 64, 32,
            39,  7, 47, 15, 55, 23, 63, 31,
            38,  6, 46, 14, 54, 22, 62, 30,
            37,  5, 45, 13, 53, 21, 61, 29,
            36,  4, 44, 12, 52, 20, 60, 28,
            35,  3, 43, 11, 51, 19, 59, 27,
            34,  2, 42, 10, 50, 18, 58, 26,
            33,  1, 41,  9, 49, 17, 57, 25
        };
            string ans = "";
            for (int i = 0; i < permutationTable.Length; ++i)
            {
                ans += s[permutationTable[i] - 1];
            }
            return ans;
        }
        public static string generateRightHalf(string oldLeft, string oldRight, string subKey)
        {
            string res = "";
            int[] Exp_Table ={32, 1, 2, 3, 4, 5,
                          4, 5, 6, 7, 8, 9,
                          8, 9, 10, 11, 12, 13,
                          12, 13, 14, 15, 16, 17,
                          16, 17, 18, 19, 20, 21,
                          20, 21, 22, 23, 24, 25,
                          24, 25, 26, 27, 28, 29,
                          28, 29, 30, 31, 32, 1};
            Console.WriteLine("Right Half: " + oldRight);
            string Exp_R0 = Expantion(oldRight, Exp_Table);
            Console.WriteLine("subkey: " + subKey);
            Console.WriteLine("Exp_R0: " + Exp_R0);
            string afterXOR = xor(subKey, Exp_R0).ToString();
            Console.WriteLine("after Xor between Exp_R0 and subkey: " + afterXOR);
            string after_calcSbox = sub_boxes(afterXOR);
            Console.WriteLine("after_calcSbox: " + after_calcSbox);
            string final_permuted = Perm(after_calcSbox);
            Console.WriteLine("after_permutaion: " + final_permuted);
            string after_final_xor = xor(oldLeft, final_permuted).ToString();
            Console.WriteLine("after_final_xor: " + after_final_xor);
            Console.WriteLine("---------------------------------------");
            return after_final_xor;
        }
        public static string BinaryToHex(string binary)
        {
            while (binary.Length % 4 != 0)
            {
                binary = "0" + binary;
            }
            string hex = "";
            for (int i = 0; i < binary.Length; i += 4)
            {
                string fourBits = binary.Substring(i, 4);
                int decimalValue = Convert.ToInt32(fourBits, 2);
                string hexDigit = decimalValue.ToString("X");
                hex += hexDigit;
            }
            return hex;
        }
        public override string Decrypt(string cipherText, string key)
        {
            // initial permutation
            string init_permuted_Key = init_perm(cipherText);
            Console.WriteLine("init_permuted_Key: " + init_permuted_Key);
            // split to L0 and R0
            int len = init_permuted_Key.Length / 2;
            Console.WriteLine("Length: " + len);
            string L0 = init_permuted_Key.Substring(0, len);
            Console.WriteLine("L0: " + L0);
            string R0 = init_permuted_Key.Substring(32, len);
            Console.WriteLine("R0: " + R0);
            // loop for Rounds 
            List<string> keyList = DES_keySchedule(key);

            List<string> left = new List<string>();
            List<string> Right = new List<string>();
            int j = 15;
            for (int i = 0; i < 16; i++)
            {
                string L1 = R0;
                left.Add(L1);
                string R1 = generateRightHalf(L0, R0, keyList[j]);
                Right.Add(R1);
                L0 = L1;
                R0 = R1;
                j--;
            }
            string L16 = left[15];
            string R16 = Right[15];
            Console.WriteLine("L16: " + L16);
            Console.WriteLine("R16: " + R16);

            string resAfter16Rounds = R16 + L16;
            Console.WriteLine("resAfter16Rounds: " + resAfter16Rounds);

            string plainText = final_Perm(resAfter16Rounds);
            Console.WriteLine("cipher: " + plainText);
            string final_plainText = "0x" + BinaryToHex(plainText);
            Console.WriteLine("final_cipher: " + final_plainText);
            return final_plainText;
        }

        public override string Encrypt(string plainText, string key)
        {
            // initial permutation
            string init_permuted_Key = init_perm(plainText);
            Console.WriteLine("init_permuted_Key: " + init_permuted_Key);
            // split to L0 and R0
            int len = init_permuted_Key.Length / 2;
            Console.WriteLine("Length: " + len);
            string L0 = init_permuted_Key.Substring(0, len);
            Console.WriteLine("L0: " + L0);
            string R0 = init_permuted_Key.Substring(32, len);
            Console.WriteLine("R0: " + R0);
            // loop for Rounds 
            List<string> keyList = DES_keySchedule(key);

            List<string> left = new List<string>();
            List<string> Right = new List<string>();
            for (int i = 0; i < 16; i++)
            {
                string L1 = R0;
                left.Add(L1);
                string R1 = generateRightHalf(L0, R0, keyList[i]);
                Right.Add(R1);
                L0 = L1;
                R0 = R1;
            }
            string L16 = left[15];
            string R16 = Right[15];
            Console.WriteLine("L16: " + L16);
            Console.WriteLine("R16: " + R16);

            string resAfter16Rounds = R16 + L16;
            Console.WriteLine("resAfter16Rounds: " + resAfter16Rounds);

            string cipher = final_Perm(resAfter16Rounds);
            Console.WriteLine("cipher: " + cipher);
            string final_cipher = "0x" + BinaryToHex(cipher);
            Console.WriteLine("final_cipher: " + final_cipher);
            return final_cipher;
        }
    }
}
