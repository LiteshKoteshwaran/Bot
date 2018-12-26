using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStoreMultiDialog.Dialogs
{
    [Serializable]
    public class Cart : IDialog<object>
    {
        public string ProductName;
        public string Description;
        public long Price;


        public Task StartAsync(IDialogContext context)
        {
            //context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {

        }
        private async Task ViewCart(IDialogContext context, IAwaitable<string> result)
        {
            string activity = await result;
            try
            {
                //if (context.ConversationData.TryGetValue<List<UserCart>>(StateKeys.UserCartInfo, out UserInfo.ListuserCarts))
                //{
                //    for (int index = 0; index < UserInfo.ListuserCarts.Count; index++)
                //    {
                //        ListStoringCart.Add(UserInfo.ListuserCarts[index].ProductName + " " + UserInfo.ListuserCarts[index].Description + " of Rs." + UserInfo.ListuserCarts[index].Price);
                //    }
                //}
                //PromptDialog.Choice(context, UpdateCart, ListStoringCart, "Select to remove", "Not a valid options", 3);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}