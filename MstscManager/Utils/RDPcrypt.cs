using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;

namespace MstscManager.Utils {
    internal class RDPcrypt {
        static byte[] s_aditionalEntropy = null;
        public static void test() {
            //加密
            //string password = "";
            //string encrpyts = encrpyt(password);
            //Console.WriteLine(encrpyts);
            //解密
            //string encrpyts = "";
            //string back = dencrpyt(strToToHexByte(encrpyts));
            //Console.WriteLine(back);
        }
        public static byte[] strToToHexByte(string hexString) {
            try {
                hexString = hexString.Replace(" ", "");
                if ((hexString.Length % 2) != 0)
                    hexString += " ";
                byte[] returnBytes = new byte[hexString.Length / 2];
                for (int i = 0; i < returnBytes.Length; i++)
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                return returnBytes;
            } catch {
                return null;
            }
        }
        public static string encrpyt(string password) {
            byte[] secret = Encoding.Unicode.GetBytes(password);
            byte[] encryptedSecret = Protect(secret);
            string res = string.Empty;
            foreach (byte b in encryptedSecret) {
                res += b.ToString("X2");
            }
            return res;
        }
        public static string dencrpyt(byte[] encryptedSecret) {
            byte[] originalData = Unprotect(encryptedSecret);
            string str = Encoding.Default.GetString(originalData);
            return str;
        }
        //加密方法
        public static byte[] Protect(byte[] data) {
            try {
                return ProtectedData.Protect(data, s_aditionalEntropy, DataProtectionScope.LocalMachine);
            } catch (CryptographicException e) {
                return null;
            }
        }
        //解密方法
        public static byte[] Unprotect(byte[] data) {
            try {
                return ProtectedData.Unprotect(data, s_aditionalEntropy, DataProtectionScope.LocalMachine);
            } catch (CryptographicException e) {
                return null;
            }
        }
        public static void PrintValues(Byte[] myArr) {
            foreach (Byte i in myArr) {
                Console.Write("\t{0}", i);
            }
            Console.WriteLine();
        }
    }
}
