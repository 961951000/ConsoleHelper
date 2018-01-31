using System;
using System.Linq;
using ConsoleHelper.Helpers;
using System.Threading.Tasks;
using Octokit;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using SimpleInjector;
using Microsoft.AspNet.SignalR;
using ConsoleHelper.Diagnostics.Entities;
using Microsoft.ApplicationInsights.DataContracts;
using ConsoleHelper.Diagnostics.Shared;

namespace ConsoleHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Guid.NewGuid());
            Console.ReadLine();
        }
    }
}
