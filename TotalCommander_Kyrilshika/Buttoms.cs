using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander_Kyrilshika
{
    class Buttoms
    {
        public ListView view;

      
        public void ButtomsNames()
        {
            Console.SetCursorPosition(5, 20);
            string buttoms = "F1-Copy | F2-Cut | F3-Paste | F4-Root | F5-List of discks | F6-Properties | F7-Rename | F8-Find | F9-New folder";
            Console.WriteLine(buttoms);
        }
        public void Update(ConsoleKeyInfo keyInfo, DirectoryListView active, DirectoryListView left, DirectoryListView right)
        {
            if (keyInfo.Key == ConsoleKey.F5)
                F5_DiscsView(active);
        }

        public void Render()
        {
            view.Render();
        }
        public DriveInfo[] F5_DiscsView(DirectoryListView active)
        {
            var view = active;
            DriveInfo[] drives = DriveInfo.GetDrives();
            active.view.Clean();
            active.view.Items = drives.Select(f => new ListViewItem(f.RootDirectory, f.Name)).ToList();
            return drives;
        }

    }
}
