using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Environment;

namespace MstscManager.Utils {
    public static class DbInihelper {

        private const int StringBufferSize = 65536;
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        
        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern long GetPrivateProfileString(string section, string key, string def, Byte[] retVal, int size, string filePath);
        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileStringA(string section, string key, string def, Byte[] retVal, int size, string filePath);

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileSectionNames")] 
        private static extern int GetPrivateProfileSectionNames(byte[] retval, int size, string filePath);

        [DllImport("kernel32")] 
        private static extern int GetPrivateProfileSection(string section, byte[] retval, int size, string filePath);
        [DllImport("kernel32")] 
        private static extern long WritePrivateProfileSection(string section, StringBuilder? retval, string filePath);

        public static bool SetIniData(string section, string key, string val, string iniFilePath) {
            if (!File.Exists(iniFilePath)) CreateFile(iniFilePath);
            long opSt = WritePrivateProfileString(section, key, val, iniFilePath);
            if (opSt == 0) { return false; } else { return true; }
        }

        public static string? GetIniData(string section, string key, string iniFilePath) {
            if (!File.Exists(iniFilePath)) { CreateFile(iniFilePath); return null; }
            List<string> result = new List<string>();
            Byte[] buffer = new Byte[65536];
            long k = GetPrivateProfileString(section, key, string.Empty, buffer, buffer.Length, iniFilePath);
            if (k == 0) return null;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("GB2312").GetString(buffer,0,Convert.ToInt32(k));
        }

        public static bool DelIniDat(string section, string key, string iniFilePath) {
            if (!File.Exists(iniFilePath)) { CreateFile(iniFilePath); return false; }
            long opSt = WritePrivateProfileString(section, key, null, iniFilePath);
            if (opSt == 0) { return false; } else { return true; }
        }

        public static List<string>? GetIniAllSectionsNames(string iniFilePath) {
            if (!File.Exists(iniFilePath)) { CreateFile(iniFilePath); return null; }
            List<string> result = new List<string>();
            Byte[] buffer = new Byte[65536];
            long len = GetPrivateProfileSectionNames(buffer, buffer.Length, iniFilePath);
            if(len == 0) return null;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buffer[i] == 0) {
                    result.Add(Encoding.GetEncoding("GB2312").GetString(buffer, j, i - j));
                    j = i + 1;
                }
            return result;
        }

        public static List<string> GetIniAllSectionsNames2(string iniFilename) {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringA(null, null, null, buf, buf.Length, iniFilename);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0) {
                    result.Add(Encoding.GetEncoding("GB2312").GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }

        public static Dictionary<string,string>? GetIniSection(string section, string iniFilePath) {
            if (!File.Exists(iniFilePath)) { CreateFile(iniFilePath); return null; }
            Dictionary<string,string> dict = new Dictionary<string,string>();
            List<string> result = new List<string>();
            Byte[] buffer = new Byte[65536];
            long k = GetPrivateProfileSection(section, buffer, buffer.Length, iniFilePath);
            if (k == 0) return null;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            int j = 0;
            for (int i = 0; i < k; i++)
                if (buffer[i] == 0) {
                    result.Add(Encoding.GetEncoding("GB2312").GetString(buffer, j, i - j));
                    j = i + 1;
                }
            foreach (var keypair in result) {
                string[] keyvalue = keypair.ToString().Split("=", StringSplitOptions.RemoveEmptyEntries);
                dict[keyvalue[0]] = keyvalue[1];
            }
            return dict;
        }

        public static List<string> GetIniSectionKeys(string SectionName, string iniFilename) {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringA(SectionName, null, null, buf, buf.Length, iniFilename);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0) {
                    result.Add(Encoding.GetEncoding("GB2312").GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }

        public static bool SetIniSection(string section,Dictionary<string,string> listKeyValue, string iniFilePath) {
            if (!File.Exists(iniFilePath)) CreateFile(iniFilePath);
            string lpString = "";
            foreach (var temp in listKeyValue.Keys) {
                lpString += (temp + "=" + listKeyValue[temp]);
                lpString += "\0";
            }
            lpString += "\0";
            long opSt = WritePrivateProfileSection(section, new StringBuilder(lpString), iniFilePath);
            if (opSt == 0) { return false; } else { return true; }
        }

        public static bool DelIniSection(string section, string iniFilePath) {
            if (!File.Exists(iniFilePath)) { CreateFile(iniFilePath); return false; }
            long opSt = WritePrivateProfileSection(section, null, iniFilePath);
            if (opSt == 0) { return false; } else { return true; }
        }

        public static void CreateFile(string path) {
            if (!string.IsNullOrEmpty(path)) {
                try {
                    string? dr = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dr)) { _ = Directory.CreateDirectory(dr); }
                    if (!File.Exists(path)) {
                        FileStream fs = File.Create(path);
                        fs.Close();
                    }
                } catch (Exception e) {
                    Console.WriteLine("CreateFile()" + e.ToString());
                }
            }
        }

    }
}
