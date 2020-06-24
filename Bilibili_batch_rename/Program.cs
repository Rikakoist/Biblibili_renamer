using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Bilibili_batch_rename
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the directory: ");
            try
            {
                string dir = Console.ReadLine();
                List<string> dirList = new List<string>();
                Common.GetAllDir(dir, ref dirList);

                int i = 0;

                foreach (string s in dirList)
                {
                    Console.Write("Rename video file in {0}...", s);
                    if (Common.Rename(s))
                    {
                        Console.Write("success.\r\n");
                        i++;
                    }
                    else
                    {
                        Console.Write("failed.\r\n");
                    }
                }
                Console.WriteLine("Total: {0}, success: {1}, fail: {2}", dirList.Count,i,dirList.Count-i);
                Console.ReadKey();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                Console.ReadKey();
                Environment.Exit(-1);
            }
        }
    }
}
