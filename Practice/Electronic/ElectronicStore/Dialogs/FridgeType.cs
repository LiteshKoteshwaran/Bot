using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStore.Dialogs
{
    [Serializable]
    public class FridgeType : IDialog<object>
    {
        const string Type1 = "Single Door";
        const string Type2 = "Double Door";
        public Task StartAsync(IDialogContext context)
        {

            this.fridgeType(context);
            return Task.CompletedTask;
        }
        private void fridgeType(IDialogContext context)
        {
            PromptDialog.Choice(context, OptionSelected, new List<string>() { Type1, Type2 }, "Select the Type of Fridge you Need", "Not a valid options", 3);


        }
        private async Task OptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;
            switch (optionSelected)
            {
                case Type1:
                    context.Call(new SingleDoor(), this.MessageReceivedAsync);
                    break;
                case Type2:
                    context.Call(new DoubleDoor(), this.MessageReceivedAsync);
                    break;
            }
        }

        private Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            throw new NotImplementedException();
        }
    }
}