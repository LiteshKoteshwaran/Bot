using System;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace _12Dec.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        static int UserScore, BotScore;
        bool AskForName = false;
        public static string RandomString(string[] chars)
        {
            Random random = new Random();

            return chars[random.Next(chars.Length)];
        }
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            activity.Text.ToLower();
            if (activity.Text == "hey" || activity.Text == "hai" || activity.Text == "hello")
            {
                //await context.PostAsync("hey");
                await context.PostAsync("Hey, Can I Know your Name Please");
            }
            else if (!AskForName)
            {
                AskForName = true;
                var UserName = activity.Text;
                await context.PostAsync($"Hi {UserName} do you wanna play Rock Paper Seissor");
            }
            if (activity.Text == "yes")
            {
                await context.PostAsync("Lets start");
                    context.Wait(Choice);
            }
            else if (activity.Text == "no")
            {
                await context.PostAsync("It's Okay thanks");
            }

        }


        //private Task Name(IDialogContext context, IAwaitable<object> activity)
        //{
        //    PromptDialog.Text(context,Option,$"Hi You wanna play? ");
        //    return Task.CompletedTask;
        //}

        //private async Task Option(IDialogContext context, IAwaitable<object> result)
        //{
        //    var reply = await result as Activity;
        //    if (reply.Equals("Yes")|| reply.Equals("yes"))
        //    {
        //        context.Wait(Choice);
        //        //PromptDialog.Text(context, Name, @"Hey, Can I Konw your Name")
        //        ////context.Wait(Choice);
        //    }
        //    //else if (activity.ToString() == "No" || activity.ToString() == "no")
        //    //{
        //    //     context.PostAsync("Its ok Thank you");
        //    //}
        //}


        private async Task Choice(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            int index = 0;
            string BotChoice;
            string[] chars = new string[3];
            chars[0] = "Rock";
            chars[1] = "Paper";
            chars[2] = "Seissor";
            for (int i = 0; i < 5; i++)
            {
                if (activity.Text == "Rock" || activity.Text == "Paper" || activity.Text == "Seissor")
                {
                    index++;
                    switch (activity.Text)
                    {
                        case ("Rock"):
                            BotChoice = RandomString(chars);
                            if (BotChoice == "Seissor")
                                BotScore--; UserScore++;
                            if (BotChoice == "Paper")
                                BotScore++; UserScore--;
                            break;
                        case ("Paper"):
                            BotChoice = RandomString(chars);
                            if (BotChoice == "Seissor")
                                BotScore++; UserScore--;
                            if (BotChoice == "Rock")
                                BotScore--; UserScore++;
                            break;
                        case ("Seissor"):
                            BotChoice = RandomString(chars);
                            if (BotChoice == "Rock")
                                BotScore++; UserScore--;
                            if (BotChoice == "Paper")
                                BotScore--; UserScore++;
                            break;
                    }
                    await context.PostAsync("Once again");
                    context.Wait(MessageReceivedAsync);
                }
                if (index == 5)
                {
                    await context.PostAsync($"Result --> UserScore: {UserScore}, BotScore: {BotScore}");
                }
            }
        }
    }
}