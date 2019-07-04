using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander_Kyrilshika
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            var left = new DirectoryListView(10, 2, 20, Console.WindowWidth / 2);
            var right = new DirectoryListView(40, 2,20, Console.WindowWidth / 2);

            while (true)
            {
                var key = Console.ReadKey();
                left.Update(key);
                left.Render();

                right.Update(key);
                right.Render();
            }
        }
    }
}
