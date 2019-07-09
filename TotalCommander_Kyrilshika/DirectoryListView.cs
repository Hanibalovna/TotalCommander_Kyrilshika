using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander_Kyrilshika
{
    class DirectoryListView
    {
        public ListView view;

        public DirectoryListView(int x, int y, int height, int width)
        {
            this.view = new ListView(x, y, height);
            view.ColumnsWidth = new List<int> { width / 10 * 7, width / 10, width / 10 * 2 };

            view.Items = GetItems("C:\\");
            view.UserState = new DirectoryInfo("C:\\");
            view.Selected += View_Selected;
            view.GoBack += View_GoBack;
        }

        public void Update(ConsoleKeyInfo keyInfo)
        {
            view.Update(keyInfo);
        }

        public void Render()
        {
            view.Render();
        }

        private static List<ListViewItem> GetItems(string path)
        {
            if (path == null)
            {
                var list = new List<ListViewItem>();
                foreach (var item in AllDrives())
                {
                    list.Add(new ListViewItem(item.RootDirectory, item.Name, "<disc>", ""));
                }
                return list;
            }
            return new DirectoryInfo(path).GetFileSystemInfos()
                .Select(f => new ListViewItem(
                    f,
                    f.Name,
                    f is DirectoryInfo dir ? "<dir>" : f.Extension,
                    f is FileInfo file ? GetPrettyLength(file.Length) : ""
                    )).ToList();
        }

        private static string GetPrettyLength(long length)//1 Кбайт = 1024 байт, 1 Мбайт = 1024 Кбайт, 1 Гбайт = 1024 Мбайт
        {

            string size = " ";
            int delitel = 1024;
            if (length > delitel && length < Math.Pow(delitel, 2))
            {
                length /= delitel;
                size = length.ToString();
                return size + "Kb";
            }
            else if (length > Math.Pow(delitel, 2) && length < Math.Pow(delitel, 3))
            {
                length = length / (long)Math.Pow(delitel, 2);
                size = length.ToString();
                return size + "Mb";
            }
            else if (length > Math.Pow(delitel, 3) && length < Math.Pow(delitel, 4))
            {
                length = length / (long)Math.Pow(delitel, 3);
                size = length.ToString();
                return size + "Gb";
            }
            else
                return size;
        }

        private static DriveInfo[] AllDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            return drives;
        }

        private static void View_Selected(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view.SelectedItem.State;
            if (info is FileInfo file)
                Process.Start(file.FullName);
            else if (info is DirectoryInfo dir)
                Navigation(dir, view);
        }
        private static void View_GoBack(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var current = (DirectoryInfo)view.UserState;
            var dir = current.Parent;
            view.Clean();
            if (dir == null)
                view.Items = GetItems(null);
            else
            {
                view.Items = GetItems(dir.FullName);
                view.UserState = dir;
            }
            // view.Items = GetItems(dir == null ? null : dir.FullName);
        }
        private static void Navigation(DirectoryInfo dir, ListView view)
        {
            try
            {
                var items = GetItems(dir.FullName);

                view.Clean();
                view.Items = items;
                view.UserState = dir;
            }
            catch
            {
                Console.WriteLine("PALUNDRA!");
            }
        }
    }
}
