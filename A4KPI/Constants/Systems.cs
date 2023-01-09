using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Constants
{
    public class Systems
    {
        public static string Administrator = "SYSTEM";
        public static string SupremeAdmin = "SUPREME";

    }

    public class JsonCLass
    {
        public string id { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string Key { get; set; }
        public string Valu { get; set; }
    }

}
