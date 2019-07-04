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
            return new DirectoryInfo(path).GetFileSystemInfos()
                .Select(f => new ListViewItem(
                    f,
                    f.Name,
                    f is DirectoryInfo dir ? "<dir>" : f.Extension,
                    f is FileInfo file ? file.Length.ToString() : ""
                    )).ToList();
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
            if (dir == null)
                view.Items = GetItems(current.FullName);
            else
                Navigation(dir, view);
        }
        private static void Navigation(DirectoryInfo dir, ListView view)
        {
            view.Clean();
            view.Items = GetItems(dir.FullName);
            view.UserState = dir;
        }
    }
}
