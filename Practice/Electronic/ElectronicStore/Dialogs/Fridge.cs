using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStore.Dialogs
{
    [Serializable]
    public class Fridge : IDialog<object>
    {
        public  Task StartAsync(IDialogContext context)
        {
            context.PostAsync("Please Select the type of Fridge you are interedted in buying");
            FridgeType fridge = new FridgeType();
           fridge.StartAsync(context);
           // context.Wait(this.MessageReceivedAsync);
            return Task.CompletedTask;
        }
        private  Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            throw new NotImplementedException();
        }

    }

}