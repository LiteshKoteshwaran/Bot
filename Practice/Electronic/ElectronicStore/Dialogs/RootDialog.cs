using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ElectronicStore.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public static string userName;
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
           
            bool IsNameAvailable = false;
            context.UserData.TryGetValue("Name", out userName);
            context.UserData.TryGetValue("GetName", out IsNameAvailable);
            if (IsNameAvailable)
            {
                userName = message.Text;
                context.UserData.SetValue("Name", userName);
                context.UserData.SetValue("GetName", false);
            }
            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("Hello, Welcome to the Electronics Store /n Please enter your Name to Continue.");
                context.UserData.SetValue("GetName", true);
            }
            else
            {
                await context.PostAsync(String.Format("Welcome {0}", userName));
                await context.PostAsync(String.Format("Please select the item you are intereted in buying"));
                //PromptDialog.Choice(context, OptionSelected, new List<string>() { ItemName0, ItemName1 }, "Are you looking for Corporate or Online training?", "Not a valid options", 3);
                Item item = new Item();
                await item.StartAsync(context);
            }





            //context.Wait(MessageReceivedAsync);
        }
        

    }
}