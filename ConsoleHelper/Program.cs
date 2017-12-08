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
            var result = FileHelper.GetAllDirectory(new DirectoryInfo(@"D:\Repos\MicrosoftDocs\docs")).Result.ToList();
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Restart();
            var result1 = FileHelper.GetAllFiles(new DirectoryInfo(@"D:\Repos\MicrosoftDocs\docs")).Result.ToList();
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Stop();
            Console.WriteLine("End...");
            Console.Read();
        }
    }
}
