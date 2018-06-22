using POEDuplicateScanner.Helpers;
using POEStashSorterModels;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace POEDuplicateScanner
{
    /// <summary>
    /// Interaction logic for QuickyWindow.xaml
    /// </summary>
    public partial class QuickyWindow : Window
    {
        private bool IsMinimized;
        public TabManager TabManager { get; set; }
        public League League { get; set; }

        public QuickyWindow()
        {
            InitializeComponent();
        }

        public void InitTabs()
        {
            ddlTabs.ItemsSource = TabManager.CurrentStash.tabs;
            ddlTabs.SelectedItem = TabManager.CurrentStash.tabs.First();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            if(IsMinimized)
            {
                this.Width = 380;
                this.Height = 140;
                btnMinimize.Content = "<";
            }
            else
            {
                this.Width = 150;
                this.Height = 65;
                btnMinimize.Content = ">";
            }

            IsMinimized = !IsMinimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            var league = this.League;
            var tab = (CustomTab) ddlTabs.SelectedItem;

            TabManager.SetLeague(league);
            TabManager.SetCurrentTab(tab);

            var runDown = TabManager.GetQuickRundown();

            TabManager.SaveTab(tab.Index);
            TabManager.CurrentTab = tab;

            lbl.Content = runDown;
            ApplicationHelper.OpenPathOfExile();
        }

        private void btnChaos_Click(object sender, RoutedEventArgs e)
        {
            TabManager.AcquireChaosSet(TabManager.CurrentTab);
        }
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private void btnRegal_Click(object sender, RoutedEventArgs e)
        {
            
           
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();


           
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // code goes here
            String path = @"D:\Appz\PathOfExile\logs\Client.txt";
            ReadTail(path);
        }
        private void ReadTail(string filename)
        {
            using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Seek 1024 bytes from the end of the file
                fs.Seek(-1024, SeekOrigin.End);
                // read 1024 bytes
                byte[] bytes = new byte[1024];
                fs.Read(bytes, 0, 1024);
                // Convert bytes to string
                string s = Encoding.Default.GetString(bytes);
                // or string s = Encoding.UTF8.GetString(bytes);
                // and output to console
                lblLogs.Content = s;
            }
        }
    }
}
