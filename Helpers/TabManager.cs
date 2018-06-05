using POEStashSorterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POEDuplicateScanner.Helpers
{
    public class TabManager
    {
        private League League;
        public void SetLeague(League league)
        {
            this.League = league;
        }



        public String GetRunDown(int tabIndex)
        {
            // Get tab
            //var tab = POEConnect.GetTab(tabIndex, this.League);
            // Mock
            var tab = POEConnect.GetMockTab(tabIndex, this.League);

            // get all items in tab.
            string result = "";
            var qry = tab.Items.GroupBy(p => p.SubType);
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

            var minArmorCount = qry.Where(p => armors.Contains(p.Key.ToString())).Min(p => p.Count());
            var minWeaponCount = ohWeapons.Where(p => p.Key == Hand.onehand).Sum(q => q.Count()) / 2;
            minWeaponCount += ohWeapons.Where(p => p.Key == Hand.twohand).Sum(q => q.Count());
            var minRingsCount = qry.Where(p => p.Key == CustomSubType.ring).Sum(q => q.Count());


            result += "Max Chaos Recipe: ";
            result += new int[] { minArmorCount, minWeaponCount, minRingsCount }.Min();

            //result += Environment.NewLine + "-----------" + Environment.NewLine;
            //result += this.GetChaosSet(2);
            //result += Environment.NewLine + "-----------" + Environment.NewLine;

            // quality
            result += "Quality > 10" + Environment.NewLine;
            result += tab.Items.Where(p => p.Properties != null && p.Properties.Exists(pi => pi.Name == "Quality")).Count();
            result += Environment.NewLine + "-----------" + Environment.NewLine;

            return result;
        }

        public string GetChaosSet(int tabIndex)
        {
            List<CustomItem> chaosSet = GetChaosItems(tabIndex);


            return string.Join(", ", chaosSet.Select(p => p.FullName + "@" + p.X + "," + p.Y));
        }

        private List<CustomItem> GetChaosItems(int tabIndex)
        {
            var tab = POEConnect.GetMockTab(tabIndex, this.League);
            // get one of each 
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

        public void AcquireChaosSet(int tabIndex)
        {
            List<CustomItem> chaosSet = GetChaosItems(tabIndex);
            var cellHeight = 20;
            var startX = 100;
            var startY = 100;

            var offsetX = cellHeight /2;
            var offsetY = cellHeight /2;
            foreach (var item in chaosSet)
            {
                var itemVector = new Vector2((item.X * cellHeight) + startX + offsetX, (item.Y * cellHeight) + startY + offsetY);
                MouseTools.MoveCursor(MouseTools.GetMousePosition(), itemVector);
                Thread.Sleep(1000);
                //MouseTools.MouseClickEvent();
                Console.WriteLine(itemVector.X + ", " + itemVector.Y);
            }
        }
    }
}
