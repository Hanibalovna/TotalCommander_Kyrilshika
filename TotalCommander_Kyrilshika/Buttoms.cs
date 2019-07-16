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
            if (keyInfo.Key == ConsoleKey.F1)
                F1_Copy(active);
            if (keyInfo.Key == ConsoleKey.F3)
                F3_Paste(active);
            if (keyInfo.Key == ConsoleKey.F4)
                F4_Root(active);
            if (keyInfo.Key == ConsoleKey.F5)
                F5_DiscsView(active);
            if (keyInfo.Key == ConsoleKey.F6)
                F6_Properties(active);
            if (keyInfo.Key == ConsoleKey.F7)
                F7_Rename(active);
            if (keyInfo.Key == ConsoleKey.F9)
                F9_NewFolder(active);
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
        public void F3_Paste(DirectoryListView active)
        {
            var f = (DirectoryInfo)active.view.UserState;
            string from = f.FullName.ToString();
            string to = buffer.FullName;
            F3_Paste_Recursion(from, to);
        }
        private void F3_Paste_Recursion(string from, string to)
        {
            Directory.CreateDirectory(to + "\\" + Path.GetFileName(from));
            foreach (string s1 in Directory.GetFiles(from))
            {
                string s2 = to + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(from))
            {
                F3_Paste_Recursion(s, to + "\\" + Path.GetFileName(s));
            }
        }
        public void F9_NewFolder(DirectoryListView active)
        {
            var p = (DirectoryInfo)active.view.UserState;
            string path = p.FullName.ToString();
            
            string subPath = "Test New Folder";

            Create(path, subPath);
        }
        public void Create(string path, string subPath)
        {
            int number = 1;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
                dirInfo.CreateSubdirectory(subPath);
            }
            else if (dirInfo.Exists)
            {
                do
                {
                    dirInfo.Create();
                    dirInfo.CreateSubdirectory(subPath + number);
                }
                while (!dirInfo.Exists);
            }
        }
        private void F7_Rename(DirectoryListView active)
        {
            //Папка, которую переименовываем
            var p = (DirectoryInfo)active.view.UserState;
            string currentFolderName = p.FullName.ToString();
            //Новое имя папки
            Console.SetCursorPosition(5, 35);
            Console.WriteLine("New Folder Name:");
            string newFolderName = Console.ReadLine();
            DirectoryInfo drInfo = new DirectoryInfo(currentFolderName);
            if (drInfo.Exists)
            {
                drInfo.MoveTo(newFolderName);
            }
        }
    }
}
