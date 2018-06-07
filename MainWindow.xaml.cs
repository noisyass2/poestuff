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
        TabManager tmgr = new TabManager();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            
            txtDups.Text = "Logging in" + Environment.NewLine;
            var sessid = txtSession.Text;
            POEConnect.Login(sessid,txtUname.Text);
            txtDups.Text += "Done" + Environment.NewLine;
            txtDups.Text += "---------------" + Environment.NewLine;

            txtDups.Text += "Getting Characters and Leagues" + Environment.NewLine;
            var leagues = POEConnect.GetLeagues();
            ddlLeagues.ItemsSource = leagues;
            ddlLeagues.SelectedItem = leagues.First();
            tmgr.SetLeague(leagues.First());
            txtDups.Text += "Done" + Environment.NewLine;
            txtDups.Text += "---------------" + Environment.NewLine;

            txtDups.Text += "Getting Tabs" + Environment.NewLine;
            tmgr.InitTabs();
            ddlLeagues_Copy.ItemsSource = tmgr.CurrentStash.tabs;
            ddlLeagues_Copy.SelectedItem = tmgr.CurrentStash.tabs.First();
            txtDups.Text += "Done" + Environment.NewLine;
            txtDups.Text += "---------------" + Environment.NewLine;

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
                var fulltab = POEConnect.GetItems(tab.Index, league);
                foreach (var item in fulltab.Items)
                {
                    allitems.Add(new SimpleItem()
                    {
                        Name = item.Name,
                        x = item.X,
                        y = item.Y,
                        tab = fulltab.Name
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var league = (League)ddlLeagues.SelectedItem;
            var tab = (CustomTab)ddlLeagues_Copy.SelectedItem;

            tmgr.SetLeague(league);
            tmgr.SetCurrentTab(tab);

            txtDups.Text = tmgr.GetRunDown();
            tmgr.SaveTab(tab.Index);
            tmgr.CurrentTab = tab;

            //Clipboard.SetText(txtDups.Text);
            ApplicationHelper.OpenPathOfExile();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            tmgr.AcquireChaosSet(tmgr.CurrentTab);
            
        }

        private void ddlLeagues_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = (CustomTab)ddlLeagues_Copy.SelectedItem;
            tmgr.CurrentTab = tab;
            txtDups.Text += "Current tab changed to :" + tmgr.CurrentTab.ToString();
        }
    }
}
