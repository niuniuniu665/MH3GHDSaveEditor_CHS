using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MH3GHDSaveEditor.Common
{
    public static class FileManager
    {
        /// <summary>
        /// 异步读取所有文件字节
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>null: 读取失败 <para/> byte[]: 文件数据</returns>
        public static async Task<byte[]> ReadAllBytesAsync(string path)
        {
            try
            {
                await Task.Run(() =>
                {
                    return File.ReadAllBytes(path);
                });
                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 创建路径（包括文件）
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFile(string path)
        {
            if (!string.IsNullOrEmpty(path) && !File.Exists(path))
            {
                string directoryPath = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(directoryPath)) { return; }

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                using (var fs = File.Create(path))
                {
                    // 文件创建完成后关闭流
                }
            }
        }

        /// <summary>
        /// 后台创建文件，并保证在取消文件占用后再结束
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task CreateFileAsync(string path)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(path)) { return; }
                string directoryPath = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(directoryPath)) { return; }
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                if (!File.Exists(path))
                {
                    using (var fs = File.Create(path))
                    {
                        // 文件创建完成后关闭流
                    }
                }
            });
        }

        /// <summary>
        /// 打开文件资源管理器选择一个文件
        /// </summary>
        /// <param name="filter">过滤器</param>"All Files (*.*)|*.*|Text Files (*.txt)|*.txt|Images (*.png;*.jpg)|*.png;*.jpg"
        /// <returns>所选文件的路径</returns>
        public static string ChooseFile(string title = "选择文件", string filter = "All Files|*.*")
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                FilterIndex = 1,
                Multiselect = false
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 使用文件资源管理器打开文件或目录
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="isFile">是否为文件</param>
        public static bool OpenFileOrFolderInExplorer(string path, bool isFile)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (isFile)
                {
                    if (!File.Exists(path))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true,
                });
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在文件资源管理器中选择文件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="multiselect">是否多选</param>
        /// <param name="filter">过滤器，格式模板："图片|*.png;*.jpg|文本|*.txt|PDF|*.pdf"</param>
        /// <returns>所选取的文件路径数组</returns>
        public static string[] SelectFilesInExplorer(string title, bool multiselect, string filter)
        {
            var dlg = new OpenFileDialog
            {
                Title = title,
                Multiselect = multiselect,
                Filter = filter,
                DefaultExt = "*.*",
                CheckFileExists = true,
                CheckPathExists = true,
            };
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                return dlg.FileNames;
            }
            return null;
        }
    }
}
