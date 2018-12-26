using ElectronicStoreMultiDialog.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicStoreMultiDialog
{
    [Serializable]
    public class UserInfo
    {
        public string Name;
        public string Email;
        public string Address;
        public static List<Cart> ListuserCarts = new List<Cart>();
    }
}