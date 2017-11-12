using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleHelper.Helpers
{
    class FileHelper
    {
        /// <summary>
        /// 强制删除文件
        /// </summary>
        /// <param name="fileName">要检查被那个进程占用的文件</param>
        public static void ForceDeletionFile(string fileName)
        {
            Process tool = new Process();
            tool.StartInfo.WorkingDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, @"SDK\Handle");
            tool.StartInfo.FileName = "handle.exe";
            tool.StartInfo.Arguments = fileName + " /accepteula";
            tool.StartInfo.UseShellExecute = false;
            tool.StartInfo.RedirectStandardOutput = true;
            tool.Start();
            tool.WaitForExit();
            string outputTool = tool.StandardOutput.ReadToEnd();

            string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
            foreach (Match match in Regex.Matches(outputTool, matchPattern))
            {
                Process.GetProcessById(int.Parse(match.Value)).Kill();
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public static void DeleteDirectory(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(path, false);
        }
    }
}
