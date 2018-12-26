using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicStoreMultiDialog
{
    [Serializable]
    public class StateKeys
    {
        public static readonly string UserName = "UserName";
        public static readonly string UserEmail = "UserEmail";
        public static readonly string UserAddress = "UserAddress";
        public static readonly string UserContactNo = "UserContactNo";

        public static readonly string ProductName = "ProductName";
        public static readonly string PrductDescription = "PrductDescription";
        public static readonly string ProductPrice = "ProductPrice";



        public static readonly List<string> ListOfBrands = new List<string>() { "lg", "mi", "vivo", "samsung", "whirlpool", "microsoft", "sony" };
        public static readonly List<string> ListOfCategory = new List<string>() { "home appliances", "mobiles", "miscellaneous", "tv", "fridge", "head Phones", "consoles" };
        
    }
}