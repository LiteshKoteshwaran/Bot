using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LRKElectronics
{
    [Serializable]
    public class userInfo
    {
        public string Name;
        public string Email;
        public string Address; 
        public UserCart userCarts = new UserCart();
    }
}