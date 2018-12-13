using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStore.Dialogs
{
    [Serializable]
    public class LcdLed : IDialog<object>
    {
        const string Type1 = "32 Inches";
        const string Type2 = "64 Inches";
        string optionSelected;
        public Task StartAsync(IDialogContext context)
        {
            this.TvSize(context);
            return Task.CompletedTask;
        }


     
        private void TvSize(IDialogContext context)
        {
            PromptDialog.Choice(context, OptionSelected, new List<string>() { Type1, Type2 }, "Select the Type of TV you Need", "Not a valid options", 3);


        }
        private async Task OptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            optionSelected = await result;
            context.Wait(MessageReceivedAsync);

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {           
            await context.PostAsync("You have selected " + optionSelected + " " + TVType.optionSelected + " " + Brand.optionSelected + " " + "television");
            await context.PostAsync("enter 'yes' to continue enter 'no' to reselect or 'exit' to Exit");
            await FinalDetails.DetailsAsync(context, result);
        }
    }

}