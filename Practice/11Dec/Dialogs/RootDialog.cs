using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace _11Dec.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            //// Calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

            // Return our reply to the user
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");



            //if (activity.Text == "C#" || activity.Text == "CSharp")
            //{
            //    await context.PostAsync($"Are you looking for C#");
                
            //}
            //if (activity.Text == "AI" || activity.Text == "Artificial Intelligence")
            //    await context.PostAsync($"Are you looking for AI");



            if(activity.Text.Contains("C#") && activity.Text.Contains("Institution"))
                await context.PostAsync($"Are you looking for C# Instution");

            //var reply = turnContext.Activity.CreateReply("Are you looking for C# Instution");

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction() { Title = "Yes", Type = ActionTypes.ImBack, Value = "Yes" },
                    new CardAction() { Title = "No", Type = ActionTypes.ImBack, Value = "No" },
                },

            };
            //await turnContext.SendActivityAsync(reply, CancellationToken);
            context.Wait(MessageReceivedAsync);




        }
    }
}