using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStore.Dialogs
{
    [Serializable]
    public class LG : IDialog<object>
    {
       
        public Task StartAsync(IDialogContext context)
        {
           // context.PostAsync("Select");
            context.Wait(this.MessageReceivedAsync);
            return Task.CompletedTask;
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
           // var option = await result as Activity;
           // await context.PostAsync($"You have selected {option.Text} Course");
        }


    }
}