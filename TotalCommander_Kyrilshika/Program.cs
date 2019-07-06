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
            var left = new DirectoryListView(0, 2, 20, Console.WindowWidth / 2 );
            var right = new DirectoryListView(Console.WindowWidth / 2, 2, 20, Console.WindowWidth / 2 );
            var active = left;

            Console.WindowWidth++;

            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    active = left;
                }
                if (key.Key == ConsoleKey.RightArrow)
                {
                    active = right;
                }

                active.Update(key);
                left.Render();
                right.Render();
            }
            
        }
    }
}
