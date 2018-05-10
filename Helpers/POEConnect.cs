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
    }
}
