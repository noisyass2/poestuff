
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEDuplicateScanner.Helpers
{
    public class CustomStash
    {
        public List<CustomTab> tabs { get; set; }
        public List<CustomItem> items { get; set; }

    }

    public class CustomTab
    {
        [JsonProperty(PropertyName = "n")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "i")]
        public int Index { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<CustomItem> Items { get; set; }

        public CustomItem GetItem(CustomSubType subtype)
        {
            return GetItems(subtype, 1).FirstOrDefault();
        }

        public List<CustomItem> GetItems(CustomSubType subtype,int cnt)
        {
            // get first item with subtype
            return this.Items.Where(p => p.SubType == subtype).Take(cnt).ToList();
        }

        public CustomItem GetItem(Hand hand)
        {
            return GetItems(hand, 1).FirstOrDefault();
        }

        public List<CustomItem> GetItems(Hand hand,int cnt)
        {
            return this.Items.Where(p => p.Hand == hand && p.MainType == CustomItemType.weapons).Take(cnt).ToList();
        }

        public override string ToString()
        {
            return this.Name + " [" + this.Type + "]";
        }
    }

    public class CustomItem
    {
        public string Name { get; set; }
        //public JToken Category { get; set; }
        public string typeLine { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        private string fullname;

        public string FullName
        {
            get { return Name + " " + typeLine; }
            set { fullname = value; }
        }

        public List<Property> Properties { get; set; }
        
        private JToken category;

        public JToken Category
        {
            get { return category; }
            set
            {
                category = value;
                var eType = "";
                var strType = this.category.ToString();
                string[] maintypes = new string[] { "weapons", "accessories", "armour", "jewels", "currency", "maps", "gems", "card", "flasks"};
                string[] subtypes = new string[] { "amulet","belt","ring","helmet","gloves","chest","shield","quiver","boots","abyss","twosword",
                                    "bow","dagger","staff","claw","onesword","wand","oneaxe","twoaxe","sceptre","onemace","twomace"};

                string mtype = maintypes.FirstOrDefault(p => strType.Contains(p));
                if (string.IsNullOrEmpty(mtype)) mtype = "others";
                string styp = subtypes.FirstOrDefault(p => strType.Contains(p));
                if (string.IsNullOrEmpty(styp)) styp = "others";

                eType += mtype + "/";
                eType += styp;
                this.MainType = (CustomItemType)Enum.Parse(typeof(CustomItemType), mtype);
                this.SubType = (CustomSubType)Enum.Parse(typeof(CustomSubType), styp);
                this.iType = eType;

                if (styp.Contains("two") || styp.Contains("bow") || styp.Contains("staff")) this.Hand = Hand.twohand; else this.Hand = Hand.onehand;
            }
        }


        private String iType;
        public String ItemType
        {
            get
            {
                
                return iType;
            }
            set { 
                iType = value;            

            }
        }

        public CustomItemType MainType { get; set; }
        public CustomSubType SubType { get; set; }
        public Hand Hand { get; set; }

       
    }

    public class Property
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "values")]
        public List<List<object>> Values { get; set; }

        [JsonProperty(PropertyName = "displayMode")]
        public int DisplayMode { get; set; }
    }

    public enum Hand
    {

        onehand,
        twohand
    }

    public enum CustomItemType
    {
        weapons,
        accessories,
        armour,
        jewels,
        currency, maps, gems, cards, flasks,
        others
    }

    public enum CustomSubType
    {
        amulet,
        belt,
        ring,
        helmet,
        gloves,
        chest,
        shield,
        quiver,
        boots,
        abyss,
        twosword,
        bow,
        dagger,
        staff,
        claw,
        onesword,
        wand,
        oneaxe,
        twoaxe,
        sceptre,
        onemace,
        twomace,
        others
    }

}
