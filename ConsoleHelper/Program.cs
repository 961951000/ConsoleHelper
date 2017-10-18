using AutoMapper;
using ConsoleHelper.Helper;
using ConsoleHelper.Models;
using ConsoleHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var book = new Book
            {
                Title = "十万个为什么",
                Author = new Author
                {
                    Name = "安徒生"
                }
            };
            
            var model= AutoMapperHelper.Run(book);
            Console.Read();
        }
    }
}
