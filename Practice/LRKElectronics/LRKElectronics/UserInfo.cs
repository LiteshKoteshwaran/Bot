using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LRKElectronics
{
    [Serializable]
    public class UserInfo
    {
        public string Name;
        public string Email;
        public string Address; 
        public UserCart userCarts = new UserCart();
        public static List<UserCart> ListuserCarts = new List<UserCart>();
    }
}