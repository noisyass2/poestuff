using POEDuplicateScanner.Helpers;
using POEStashSorterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POEDuplicateScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var sessid = "";
            POEConnect.Login(sessid);
        }

        private void Get_Leagues_Click(object sender, RoutedEventArgs e)
        {
            var leagues = POEConnect.GetLeagues();
            ddlLeagues.ItemsSource = leagues;
        }

        private void CheckDuplicates_Click(object sender, RoutedEventArgs e)
        {
            var league = (League) ddlLeagues.SelectedItem;

            var allitems = new List<SimpleItem>();
            // get stashes
            var tabs = POEConnect.GetTabs(league);
            foreach (var tab in tabs)
            {
                // simplify all items in one list
                foreach (var item in tab.Items)
                {
                    allitems.Add(new SimpleItem()
                    {
                        Name = item.Name,
                        x = item.X,
                        y = item.Y,
                        tab = tab.Name
                    });
                }
            }

            // get duplicates
            var dups = allitems.GroupBy(x => x.Name)
                .Where(g => g.Count() > 1)
                .Select(y => new { Name = y.Key, Items = y.ToList() });

            // show output
            foreach (var grp in dups)
            {
                txtDups.Text += grp.Name + ":" + Environment.NewLine;
                foreach (var item in grp.Items)
                {
                    txtDups.Text += "     [" + item.tab + "] @ X:" + item.x + ", Y:" + item.y + Environment.NewLine;
                }
                txtDups.Text += "--------------" + Environment.NewLine;
            }
        }


        class SimpleItem
        {
            public string Name { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public string tab { get; set; }
        }
    }
}
