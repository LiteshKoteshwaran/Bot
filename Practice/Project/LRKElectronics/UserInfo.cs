using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LRKElectronics
{
    [Serializable]
    public class UserInfo
    {
        public static string Name;
        public static string Email;
        public static string Address; 
        public UserCart userCarts;
    }
}