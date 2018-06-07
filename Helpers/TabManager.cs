using Newtonsoft.Json;
using POEStashSorterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POEDuplicateScanner.Helpers
{
    public class TabManager
    {
        public CustomStash CurrentStash;
        public CustomTab CurrentTab;

        private League League;
        public void SetLeague(League league)
        {
            this.League = league;
        }

        public void SetCurrentTab(CustomTab tab)
        {
            var newTab = POEConnect.GetTab(tab.Index, this.League);
            tab.Items = newTab.Items;
            this.CurrentTab = tab;
        }

        public String GetRunDown()
        {
            // Get tab
            var tab = CurrentTab;
            // Mock
            //var tab = POEConnect.GetMockTab(tabIndex, this.League);
            // get all items in tab.
            string result = "";
            var qryMain = tab.Items.GroupBy(p => p.MainType);
            foreach (var stype in qryMain)
            {
                result += stype.Key + ":" + stype.Count() + ", ";
            }

            result += Environment.NewLine + "-----------" + Environment.NewLine;

            
            var qry = tab.Items.Where(p => p.MainType != CustomItemType.weapons).GroupBy(p => p.SubType);
            foreach (var stype in qry)
            {
                result += stype.Key + ":" + stype.Count() + ", ";
            }

            result += Environment.NewLine + "-----------" + Environment.NewLine;

            //get weapons by hand
            var ohWeapons = tab.Items.Where(p => p.MainType == CustomItemType.weapons).GroupBy(p => p.Hand);

            result += String.Join(", ", ohWeapons.Select(p => p.Key + ":" + p.Count()));

            result += Environment.NewLine + "-----------" + Environment.NewLine;

            //chaos recipe sets
            
            var armors = new string[] { "helmet","gloves","boots","chest","amulet","belt"};

            var armorsinchest = qry.Where(p => armors.Contains(p.Key.ToString()));
            var minArmorCount = armorsinchest.Count() > 0 ? armorsinchest.Min(p => p.Count()) : 0;
            var wepinchest = ohWeapons.Where(p => p.Key == Hand.onehand);
            var minWeaponCount = wepinchest.Count() > 0 ? wepinchest.Sum(q => q.Count()) / 2 : 0;
            minWeaponCount += ohWeapons.Where(p => p.Key == Hand.twohand).Sum(q => q.Count());
            var ringinchest = qry.Where(p => p.Key == CustomSubType.ring);
            var minRingsCount = ringinchest.Count() > 0 ? ringinchest.Sum(q => q.Count()) : 0;


            result += "Max Chaos Recipe: ";
            result += new int[] { minArmorCount, minWeaponCount, minRingsCount }.Min();

            result += Environment.NewLine + "-----------" + Environment.NewLine;
            //result += this.GetChaosSet(2);
            //result += Environment.NewLine + "-----------" + Environment.NewLine;

            //// quality
            //result += "Quality > 10" + Environment.NewLine;
            //result += tab.Items.Where(p => p.Properties != null && p.Properties.Exists(pi => pi.Name == "Quality")).Count();
            //result += Environment.NewLine + "-----------" + Environment.NewLine;

            return result;
        }

        public string GetChaosSet()
        {
            List<CustomItem> chaosSet = GetChaosItems();

            return string.Join(", ", chaosSet.Select(p => p.FullName + "@" + p.X + "," + p.Y));
        }

        private  List<CustomItem> GetChaosItems()
        {
            // get one of each 
            var tab = CurrentTab;
            var helm = tab.GetItem(CustomSubType.helmet);
            var gloves = tab.GetItem(CustomSubType.gloves);
            var boots = tab.GetItem(CustomSubType.boots);
            var chest = tab.GetItem(CustomSubType.chest);
            var amulet = tab.GetItem(CustomSubType.amulet);
            var belt = tab.GetItem(CustomSubType.belt);

            // get two of each
            var rings = tab.GetItems(CustomSubType.ring, 2);
            var weapons = tab.GetItems(Hand.onehand, 2);
            if (weapons.Count() < 2) weapons = tab.GetItems(Hand.twohand, 1);


            List<CustomItem> chaosSet = new List<CustomItem>()
            {
                helm,gloves,boots,chest,
                belt,amulet
            };
            chaosSet = chaosSet.Concat(rings).Concat(weapons).ToList();
            return chaosSet;
        }

        public void SaveTab(int tabIndex)
        {
            // Get tab
            var tab = POEConnect.GetTab(tabIndex, this.League);
            File.WriteAllText("savedtab.json", JsonConvert.SerializeObject(tab));
        }

        public CustomTab LoadTab()
        {
            var json = File.ReadAllText("savedtab.json");
            return JsonConvert.DeserializeObject<CustomTab>(json);
        }

        public void AcquireChaosSet(int tabIndex)
        {
            List<CustomItem> chaosSet = GetChaosItems();
            StartAcquiring(chaosSet);

            // remove from tab

        }

        private static void StartAcquiring(List<CustomItem> chaosSet)
        {
            ApplicationHelper.OpenPathOfExile();

            var cellHeight = 26;
            var startX = 15;
            var startY = 160;

            var offsetX = cellHeight / 2;
            var offsetY = cellHeight / 2;
            foreach (var item in chaosSet)
            {
                var itemVector = new Vector2((item.X * cellHeight) + startX + offsetX, (item.Y * cellHeight) + startY + offsetY);
                MouseTools.MoveCursor(MouseTools.GetMousePosition(), itemVector);
                Thread.Sleep(200);
                MouseTools.MouseClickEvent();
                Console.WriteLine(itemVector.X + ", " + itemVector.Y);
            }
        }

        public void AcquireChaosSet(CustomTab tab)
        {
            List<CustomItem> chaosSet = GetChaosItems();
            StartAcquiring(chaosSet);
            //Remove chaosset from tab
            CurrentTab.Items = CurrentTab.Items.Except(chaosSet).ToList();
        }


        public void InitTabs()
        {
            this.CurrentStash = POEConnect.InitTabs(0, this.League);

        }
    }
}
