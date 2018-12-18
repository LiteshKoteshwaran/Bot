using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LRKElectronics
{
    [Serializable]
    public class UserCart
    {
        //public List<string> ProductName=new List<string>();
        //public List<string> Description = new List<string>();
        //public List<long> Price=new List<long>();


        public string ProductName;
        public string Description;
        public long Price;
    }
}