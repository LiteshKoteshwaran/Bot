using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStore.Dialogs
{
    [Serializable]
    public class Item : IDialog<object>
    {
        const string ItemName1 = "TV";
        const string ItemName2 = "Fridge";

        public Task StartAsync(IDialogContext context)
        {
            ProductOption(context);

            return Task.CompletedTask;
        }
        private void ProductOption(IDialogContext context)
        {
            PromptDialog.Choice(context, OptionSelected, new List<string>() { ItemName1, ItemName2 }, "Select the Product you Need", "Not a valid options", 3);
            //PromptDialog.Attachment(context,OptionSelected,new List<>)

        }

        private async Task OptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;
            switch (optionSelected)
            {
                case ItemName1:
                    context.Call(new TV(), this.MessageReceivedAsync);
                    break;
                case ItemName2:
                    context.Call(new Fridge(), this.MessageReceivedAsync);
                    break;
            }
        }

        private Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            throw new NotImplementedException();
        }
    }
}