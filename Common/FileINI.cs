using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace MH3GHDSaveEditor.Common
{
    public static class FileINI
    {
        /// <summary>
        /// 读取文件的缓冲区字节大小
        /// </summary>
        private static readonly int readBufferSize = 255;

        /// <summary>
        /// 声明INI文件的写操作函数 WritePrivateProfileString()
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>
        /// 声明INI文件的读操作函数 GetPrivateProfileString()
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <param name="retVal"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileString(string section, string key, string def, Byte[] retVal, int size, string filePath);

        /// <summary>
        /// 写入INI
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="path"></param>
        public static void Write(string section, string key, string value, string path)
        {
            // section=配置节点名称，key=键名，value=返回键值，path=路径
            long res = WritePrivateProfileString(section, key, value, path);

            if (res != 0)
            {
                Debug.WriteLine($"Write File {path} success!");
            }
            else
            {
                Debug.WriteLine($"Write File {path} failed!");
            }
        }

        /// <summary>
        /// 读取INI
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Read(string section, string key, string path)
        {
            if (!File.Exists(path))
            {
                Debug.WriteLine($"{path} not exist");
                return null;
            }
            // 每次从ini中读取多少字节
            System.Text.StringBuilder temp = new System.Text.StringBuilder(readBufferSize);

            // section=配置节点名称，key=键名，temp=上面，path=路径
            GetPrivateProfileString(section, key, "", temp, readBufferSize, path);
            return temp.ToString();
        }

        /// <summary>
        /// 以字典形式读取INI
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ReadAsDict(string path)
        {
            if (!File.Exists(path))
            {
                Debug.WriteLine($"{path} not exist!");
                return null;
            }
            return null;
        }

        public static List<string> ReadKeys(string SectionName, string iniFilename)
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileString(SectionName, null, null, buf, buf.Length, iniFilename);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }

        /// <summary>
        /// 删除一个INI文件
        /// </summary>
        /// <param name="FilePath"></param>
        public static void Delete(string FilePath)
        {
            File.Delete(FilePath);
        }
    }
}
