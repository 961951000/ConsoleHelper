using AutoMapper;
using ConsoleHelper.Models;
using ConsoleHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using ConsoleHelper.Helpers;
using System.Diagnostics;

namespace ConsoleHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();
            var result = GetAllFiles(@"D:\Repos\MicrosoftDocs\docs").ToList();
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch = Stopwatch.StartNew();
            var result1 = RecursiveAsync(new DirectoryInfo(@"D:\Repos\MicrosoftDocs\docs")).Result.ToList();
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Stop();
            Console.WriteLine("End...");
            Console.Read();
        }

        /// <summary>  
        /// 列出指定目录下及所其有子目录及子目录里更深层目录里的文件（需要递归）  
        /// </summary>  
        /// <param name="path"></param>  
        public static IEnumerable<DirectoryInfo> GetAllFiles(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            DirectoryInfo[] subDir = dir.GetDirectories();
            foreach (DirectoryInfo d in subDir)
            {
                subDir.Concat(GetAllFiles(d.FullName));
            }

            return subDir;
        }

        private async static Task<IEnumerable<FileInfo>> RecursiveAsync(DirectoryInfo root)
        {
            var tasks = root.GetDirectories().Select(RecursiveAsync);
            var files = (await Task.WhenAll(tasks)).SelectMany(x => x);
            var result = root.GetFiles().Concat(files);

            return result;
        }
    }
}
