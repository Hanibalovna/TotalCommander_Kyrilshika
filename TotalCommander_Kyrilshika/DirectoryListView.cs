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
            view.ColumnsWidth = new List<int> { width / 10 * 8, width / 10, width / 10 };

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

        private static string GetPrettyLength(long length)
        {
            string size = length.ToString();
            int delitel = 1024;
            if (length >= delitel)
            {
            length = length / delitel;
               Console.WriteLine(size + "Kb");
            return size;
            }
            else if (length >= Math.Pow(delitel,2))
                    {
                length = length / Math.Pow(delitel, 2);
                return size;
            }
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
