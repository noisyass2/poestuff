using Newtonsoft.Json;
using POEStashSorterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEDuplicateScanner.Helpers
{
    public static class POEConnect
    {
        private const string STASHURL = "http://www.pathofexile.com/character-window/get-stash-items?league={0}&tabs=1&tabIndex={1}";

        public static void Login(string sessid){
            PoeConnector.Connect("", sessid, true);
        }

        public static List<League> GetLeagues()
        {
            return PoeConnector.FetchLeagues();
        }

        public static List<Tab> GetTabs(League league)
        {
            return PoeConnector.FetchTabs(league);

        }

        public static CustomTab GetTab(int tabIndex, League league)
        {
            CustomTab tab = null;
            string jsonData = PoeConnector.WebClinet.DownloadString(string.Format(STASHURL, league.Name, tabIndex));
            CustomTab stash = JsonConvert.DeserializeObject<CustomTab>(jsonData);            
            tab.Items = stash.Items;

            return tab;
        }

        public static CustomTab GetMockTab(int tabIndex, League league)
        {
            string json = File.ReadAllText("mockquadtab.json");
            CustomTab tab= JsonConvert.DeserializeObject<CustomTab>(json);

            return tab;

        }

    }
}
