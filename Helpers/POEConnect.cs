using Newtonsoft.Json;
using POEStashSorterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEDuplicateScanner.Helpers
{
    public static class POEConnect
    {        
        public static void Login(string sessid,string uname){
            PoeConnector.Connect("", sessid, true);
            PoeConnector.UNAME = uname;
        }

        public static List<League> GetLeagues()
        {
            return PoeConnector.FetchLeagues();
        }

        public static List<Tab> GetTabs(League league)
        {
            return PoeConnector.FetchTabs(league);

        }

        internal static Tab GetItems(int index,League league)
        {
            return PoeConnector.FetchTab(index, league);
        }

        public static CustomTab GetTab(int tabIndex, League league)
        {
            CustomTab tab = null;
            string jsonData = PoeConnector.FetchTabJson(tabIndex, league);

            CustomTab stash = JsonConvert.DeserializeObject<CustomTab>(jsonData);
            //tab.Items = stash.Items;

            return stash;
        }

        public static CustomStash InitTabs(int tabIndex, League league)
        {
            
            string jsonData = PoeConnector.FetchTabJson(tabIndex, league);

            CustomStash stash = JsonConvert.DeserializeObject<CustomStash>(jsonData);
            //tab.Items = stash.Items;

            return stash;
        }
    }
}
