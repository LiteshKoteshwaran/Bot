using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStore.Dialogs
{
    [Serializable]
    public class SingleDoor : IDialog<object>
    {
        const string Type1 = "Frost Free";
        const string Type2 = "Instant Cool";
        public static string optionSelected;
        public Task StartAsync(IDialogContext context)
        {

            this.TvType(context);
            return Task.CompletedTask;
        }
        private void TvType(IDialogContext context)
        {
            PromptDialog.Choice(context, OptionSelected, new List<string>() { Type1, Type2 }, "Select the Type of TV you Need", "Not a valid options", 3);


        }
        public async Task OptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            optionSelected = await result;
            switch (optionSelected)
            {
                case Type1:
                    context.Call(new SecondType(), this.MessageReceivedAsync);
                    break;
                case Type2:
                    context.Call(new SecondType(), this.MessageReceivedAsync);
                    break;
            }


        }

        private Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            throw new NotImplementedException();
        }
    }
}