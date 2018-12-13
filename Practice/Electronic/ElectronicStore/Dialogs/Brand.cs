using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStore.Dialogs
{
    [Serializable]
    public class Brand : IDialog<object>
    {
        const string Brand1 = "LG";
        const string Brand2 = "Onida";
        public static string optionSelected;
        public Task StartAsync(IDialogContext context)
        {

            BrandSelect(context);
            return Task.CompletedTask;
        }
        private void BrandSelect(IDialogContext context)
        {
            PromptDialog.Choice(context, OptionSelected, new List<string>() { Brand1, Brand2 }, "Select the Type of TV you Need", "Not a valid options", 3);


        }
        public async Task OptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            optionSelected = await result;
            switch (optionSelected)
            {
                case Brand1:
                    context.Call(new TVType(), this.MessageReceivedAsync);
                    break;
                case Brand2:
                    context.Call(new TVType(), this.MessageReceivedAsync);
                    break;
            }


        }

        private Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            throw new NotImplementedException();
        }
    }
}