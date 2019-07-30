using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander_Kyrilshika
{
    class ListView
    {
        private int prevSelectedIndex;
        private int selectedIndex;
        private bool wasPainted;

        private int scroll;
        private int x, y, height;

        public ListView(int x, int y, int height)
        {
            this.x = x;
            this.y = y;
            this.height = height;
        }
        public ListView()
        {
        }
        public List<int> ColumnsWidth { get; set; }
        private List<ListViewItem> items;
        public List<ListViewItem> Items
        {
            get { return items; }
            set
            {
                scroll = 0;
                items = value;
            }
        }
        public object UserState { get; set; }

        public void Clean()
        {
            selectedIndex = prevSelectedIndex = 0;
            wasPainted = false;
            for (int i = 0; i < Math.Min(height, Items.Count); i++)
            {
                Items[i].Clean(ColumnsWidth, i, x, y);
            }
        }
        public ListViewItem SelectedItem => Items[selectedIndex];

        public bool Focused { get; set; }

        public void Render()
        {
            for (int i = 0; i < Math.Min(height, Items.Count); i++)
            {
                int elementIndex = i + scroll;

                if (wasPainted)
                {
                    if (elementIndex != selectedIndex && elementIndex != prevSelectedIndex)
                        continue;
                }

                var item = Items[elementIndex];
                var savedForeground = Console.ForegroundColor;
                var savedBackGround = Console.BackgroundColor;
                if (elementIndex == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.CursorLeft = x;
                Console.CursorTop = i + y;
                item.Render(ColumnsWidth, i, x, y);

                Console.ForegroundColor = savedForeground;
                Console.BackgroundColor = savedBackGround;
            }
            wasPainted = true;
        }
        public void Update(ConsoleKeyInfo key)
        {
            prevSelectedIndex = selectedIndex;

            if (key.Key == ConsoleKey.DownArrow && selectedIndex + 1 < Items.Count)
                selectedIndex++;
            else if (key.Key == ConsoleKey.UpArrow && selectedIndex - 1 >= 0)
                selectedIndex--;

            if (selectedIndex >= height + scroll)
            {
                scroll++;
                wasPainted = false;
            }
            else if (selectedIndex < scroll)
            {
                scroll--;
                wasPainted = false;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                Selected(this, EventArgs.Empty);

            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                GoBack(this, EventArgs.Empty);

            }
        }
        public event EventHandler Selected;
        public event EventHandler GoBack;

    }
}
