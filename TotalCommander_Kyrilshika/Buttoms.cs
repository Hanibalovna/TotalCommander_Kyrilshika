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
            Console.SetCursorPosition(5, 22);
            string buttoms = "F1-Copy | F2-Cut | F3-Paste | F4-Root | F5-List of discks | F6-Properties | F7-Rename | F9-New folder";
            Console.WriteLine(buttoms);
        }
        public void Update(ConsoleKeyInfo keyInfo, DirectoryListView active, DirectoryListView left, DirectoryListView right)
        {
            if (keyInfo.Key == ConsoleKey.F5)
                F5_DiscsView(active);
            if (keyInfo.Key == ConsoleKey.F4)
                F4_Root(active);
            if (keyInfo.Key == ConsoleKey.F6)
                F6_Properties(active);
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
        public void F4_Root(DirectoryListView active)
        {
            var view = active;
            var current = (DirectoryInfo)view.view.UserState;
            var root = current.Root;
            active.Navigation(root);
        }
        public void F6_Properties(DirectoryListView active)
        {
            var view = active;
            var current = view.view.SelectedItem.State;

            if (current is FileInfo file)
            {
                current = (DirectoryInfo)view.view.UserState;
                Console.SetCursorPosition(0, 25);
                Console.WriteLine("Name: " + file.Name.PadRight(80));
                Console.WriteLine("Parent Directory: " + file.DirectoryName);
                Console.WriteLine("Last Read Time: " + file.LastAccessTime);
                Console.WriteLine("Last Write Time: " + file.LastWriteTime);
                Console.WriteLine("Size: " + active.GetPrettyLength(file.Length));

            }
            else if (current is DirectoryInfo dir)
            {
                Console.SetCursorPosition(0, 25);
                Console.WriteLine("Name: " + dir.Name.PadRight(80));
                Console.WriteLine("Parent Directory: " + dir.Parent);
                Console.WriteLine("Root Directory: " + dir.Root);
                Console.WriteLine("Last Read Time: " + dir.LastAccessTime);
                Console.WriteLine("Last Write Time: " + dir.LastWriteTime);
            }
        }

        private FileSystemInfo buffer;

        public void F1_Copy(DirectoryListView active)
        {
            buffer = (FileSystemInfo)active.view.SelectedItem.State;
        }

        //public void F3_Paste(DirectoryListView active)
        //{
        //    var from = (DirectoryInfo)active.view.UserState;
        //    var to = buffer;
        //    foreach (var file in from.GetFiles())
        //        File.Copy(file.FullName, Path.Combine(to.FullName, file.Name));
        //    foreach (var papka in from.GetDirectories())
        //       Directory.CreateDirectory(papka.FullName).Copy(papka, Path.Combine(to.FullName, papka.Name));
        //}

        public void F9_NewFolder(DirectoryListView active)
        {
            Console.SetCursorPosition(5, 22);
            Console.WriteLine("Enter name of new folder");
            string folderName = Console.ReadLine();
        }
    }
}
