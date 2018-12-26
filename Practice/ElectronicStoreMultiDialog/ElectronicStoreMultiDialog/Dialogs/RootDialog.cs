using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ElectronicStoreMultiDialog.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        UserInfo userInfo = new UserInfo();
        //DAL dal;
        //static string UserName;


        public async Task StartAsync(IDialogContext context)
        {
            if (context.UserData.TryGetValue(StateKeys.UserName, out userInfo.Name))
            {
                // If the user has already logged in he will b getting    
                await context.PostAsync($"welcome back {userInfo.Name}");
                //PromptDialog.Choice(context, this.ForExitingUser, new List<string> { "Shopping", "View Cart" }, "Select Choice", "Not a valid options", 3);
                context.Wait(MessageReceivedAsync);
            }
            else
                context.Wait(MessageReceivedAsync);

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            try
            {
                if ((activity.Text == "hey" || activity.Text == "hi" || activity.Text == "hai") && !(context.UserData.TryGetValue(StateKeys.UserName, out userInfo.Name)))
                {
                    await context.PostAsync("Welcome to the Electronic Store");
                    await context.PostAsync("Please Enter your Name");
                    context.Wait(MessageReceivedAsync);
                }

                else if (!context.UserData.TryGetValue(StateKeys.UserName, out userInfo.Name))
                {
                    userInfo.Name = activity.Text;
                    context.UserData.SetValue(StateKeys.UserName, userInfo.Name);
                    await context.PostAsync($"Hey {userInfo.Name} Enter your Email to continue");
                    context.Wait(MessageReceivedAsync);
                }

                else if (!context.UserData.TryGetValue(StateKeys.UserEmail, out userInfo.Email))
                {
                    userInfo.Email = activity.Text;
                    context.UserData.SetValue(StateKeys.UserEmail, userInfo.Email);
                    await context.PostAsync("Your Address????");
                    context.Wait(MessageReceivedAsync);
                }

                else if (!context.UserData.TryGetValue(StateKeys.UserAddress, out userInfo.Address))
                {
                    userInfo.Address = activity.Text;
                    context.UserData.SetValue(StateKeys.UserAddress, userInfo.Address);
                    context.Wait(MessageReceivedAsync);
                }
                else
                {
                    await Luis.IdentifyUserQueryUsingLuis(activity.Text);
                    await IntentOperations.IdentifyUserIntent(context,result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }

        public static void Checkout(IDialogContext context, IAwaitable<object> result)
        {

        }

        private async Task HeroCard(IDialogContext context, IAwaitable<string> result)
        {
            var resultMessage = context.MakeMessage();
            resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            resultMessage.Attachments = new List<Attachment>();

            foreach (var cart in UserInfo.ListuserCarts)
            {
                HeroCard heroCard = new HeroCard()
                {
                    //Title = Luis.Entity,
                    //Subtitle = $"{cart.ProductName} starts. {cart.ProductName} reviews. From ${cart.ProductName} per night.",
                    Images = new List<CardImage>()
                    {
                        new CardImage(){Url = " "}
                    },
                    Buttons = new List<CardAction>()
                    {
                             new CardAction(ActionTypes.ImBack, "Edit", value: "Edit"),
                    }
                };
                resultMessage.Attachments.Add(heroCard.ToAttachment());
            }
            await context.PostAsync(resultMessage);
            //await Cart(context,result);
        }

    }
}