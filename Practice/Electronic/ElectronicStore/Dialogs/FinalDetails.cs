using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStore.Dialogs
{
    public class FinalDetails
    {
        public static async Task DetailsAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if(message.Text=="yes")
            {
                await context.PostAsync(String.Format("Thank You {0}", RootDialog.userName +"  "  + "you will recieve a congformation mail as your order is proceed"));
            }
            else if(message.Text=="no")
            {
                await context.PostAsync($"Thank You {0}", RootDialog.userName + "  " + " your order has been cancelled");
            }
            else if(message.Text=="exit")
            {
                await context.PostAsync($"Thank You {0}", RootDialog.userName + "  " + "see you again");
            }
        }
    }
}