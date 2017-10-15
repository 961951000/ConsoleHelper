using ConsoleHelper.Helper;
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
            WeekHelper o = new WeekHelper();
            Console.WriteLine(o.GetWeekFirstDayMon(DateTime.Now));
            Console.WriteLine(o.GetWeekLastDaySun(DateTime.Now));
            Console.Read();
        }
    }
}
