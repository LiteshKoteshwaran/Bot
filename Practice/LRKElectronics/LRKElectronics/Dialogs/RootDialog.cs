using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LRKElectronics.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        //List<UserCart> userCart = new List<UserCart>();
        UserCart cart = new UserCart();
        UserInfo userInfo = new UserInfo();
        ConnetionMannger connetionMannger;
        DAL dal = new DAL();
        List<string> ListStoringCart = new List<string>();
        public static string BrandSelected, CategorySelected, SubCategorySelected, TypeSelected, SelectedDescription;
        long Price;
        public async Task StartAsync(IDialogContext context)
        {
            if (context.UserData.TryGetValue(StateKeys.UserInformation, out userInfo.Name))
            {
                await context.PostAsync($"welcome back {userInfo.Name}");
                PromptDialog.Choice(context, this.ForExitingUser, new List<string> { "Shopping", "View Cart" }, "Select Choice", "Not a valid options", 3);
            }
            else if (!context.UserData.TryGetValue(StateKeys.UserInformation, out userInfo.Name))
            {
                context.Wait(MessageReceivedAsync);
            }
        }
        private async Task ForExitingUser(IDialogContext context, IAwaitable<string> result)
        {
            var activity = await result;
            if(activity=="Shopping")
            {
                ShowOptions(context);
            }
            else if(activity== "View Cart")
            {
                await Cart(context, result);
            }
        }
        
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            try
            {
                    if ((activity.Text == "hey" || activity.Text == "hi" || activity.Text == "hai")&& !(context.UserData.TryGetValue(StateKeys.UserInformation, out userInfo.Name)))
                    {
                        try
                        {
                            await context.PostAsync("Welcome to the Electronic Store");
                            await context.PostAsync("Please Enter your Name to continue");
                            //ResumeAfterOptionDialog(context, result);
                            context.Call(new RootDialog(), ResumeAfterOptionDialog);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                else if (!context.UserData.TryGetValue(StateKeys.UserInformation, out userInfo.Name))
                {
                    try
                    {
                        userInfo.Name = activity.Text;
                        context.UserData.SetValue(StateKeys.UserInformation, userInfo.Name);
                        await context.PostAsync($"Hey {userInfo.Name} Enter your Email to continue");
                        ResumeAfterOptionDialog(context, result);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (!context.UserData.TryGetValue("Email", out userInfo.Email))
                {
                    userInfo.Email = activity.Text;
                    context.UserData.SetValue("Email", userInfo.Email);
                    await context.PostAsync("Your Address????");
                    ResumeAfterOptionDialog(context, result);
                }
                else if (!context.UserData.TryGetValue("Address", out userInfo.Address))
                {
                    userInfo.Address = activity.Text;
                    context.UserData.SetValue("Address", userInfo.Address);
                    this.ShowOptions(context);
                }
                }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            //await this.MessageReceivedAsync(context,result);
            context.Wait(MessageReceivedAsync);
        }
        private void ShowOptions(IDialogContext context)
        {
            string Query = "select Name from Category";
            DAL dal = new DAL();
            List<string> Category = dal.GetCategory(Query);
            PromptDialog.Choice(context, SelectedCategory, Category, "Please choose one from the options", "Invalid Option type. Please try again", 3);
        }
        private async Task SelectedCategory(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                CategorySelected = await result;
                DAL dal = new DAL();
                string Query = "select distinct b.Name from  Brand b join Product p on b.Id = p.BrandId join Category c  on C.Id=p.CategoryId where c.Name ='" + CategorySelected + "'";
                List<string> Category = dal.GetCategory(Query);
                if (CategorySelected == "Mobiles")
                {
                    PromptDialog.Choice(context, SelectedBrand, Category, "Please choose one from the options", "Invalid Option type. Please try again", 3);
                }
                else
                {
                    string QueryForSubCategory = " select SubCategoryName from SubCategory join Category on SubCategory.CategoryId=Category.Id where Category.Name = '" + CategorySelected + "'";
                    List<string> SubCategory = dal.GetCategory(QueryForSubCategory);
                    PromptDialog.Choice(context, SelectedCategoryForHomeAndMisc, SubCategory, "Please choose one from the options", "Invalid Option type. Please try again", 3);
                }
            }
            catch (Exception e)
            {
                await context.PostAsync("Thanks");
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task SelectedBrand(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                BrandSelected = await result;
                DAL dal = new DAL();
                string Query = "select distinct Title from Product join Brand on Product.BrandId = Brand.Id join Category on Category.Id=Product.CategoryId where Brand.Name  = '" + BrandSelected + "'" + "and Category.Name = '" + CategorySelected + "'";
                List<string> Brand = dal.GetCategory(Query);
                PromptDialog.Choice(context, DisplaySelection, Brand, "Please choose one from the options", "Invalid Option type. Please try again", 3);
            }
            catch
            {
                await context.PostAsync("Thanks");
                context.Wait(this.MessageReceivedAsync);
            }
        }



        private async Task SelectedCategoryForHomeAndMisc(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                SubCategorySelected = await result;
                DAL dal = new DAL();
                string Query = "select distinct Brand.Name from Brand join Product on Brand.Id = Product.BrandId join Category on Category.Id = Product.CategoryId join SubCategory on SubCategory.SubCategoryId = Product.SubCategoryId where Category.Name ='" + CategorySelected + "'" + "and SubCategory.SubCategoryName = '" + SubCategorySelected + "'";
                List<string> Brand1 = dal.GetCategory(Query);
                PromptDialog.Choice(context, SelectedBarndForSubCategory, Brand1, "Please choose one from the options", "Invalid Option type. Please try again", 3);
            }
            catch (Exception ex)
            {
                await context.PostAsync("Thanks");
                context.Wait(this.MessageReceivedAsync);
            }
        }
        private async Task SelectedBarndForSubCategory(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                BrandSelected = await result;
                DAL dal = new DAL();
                string Query = "select distinct Product.Title from Product join Brand on Brand.Id = Product.BrandId join Category on Category.Id = Product.CategoryId join SubCategory on SubCategory.SubCategoryId = Product.SubCategoryId where Category.Name ='" + CategorySelected + "'" + "and SubCategory.SubCategoryName = '" + SubCategorySelected + "'" + "and Brand.Name = '" + BrandSelected + "'";
                List<string> Brand = dal.GetCategory(Query);
                PromptDialog.Choice(context, DisplaySelection, Brand, "Please choose one from the options", "Invalid Option type. Please try again", 3);
            }
            catch
            {
                await context.PostAsync("Thanks");
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task DisplaySelection(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                DAL dal = new DAL();
                connetionMannger = new ConnetionMannger();
                TypeSelected = await result;
                string Query = "select Price from Product where Title='" + TypeSelected + "'";
                Price = (long.Parse(dal.GetSelection(Query)));
                string QueryForDescription = "select p.Description from product p join brand b on B.Id = p.brandId join category c on p.CategoryId = c.Id where b.Name = '" + BrandSelected + "'" + "and c.Name = '" + CategorySelected + "'";
                SelectedDescription = dal.GetSelection(QueryForDescription);

                userInfo.userCarts.ProductName = TypeSelected;
                userInfo.userCarts.Description = SelectedDescription;
                userInfo.userCarts.Price = Price;
                context.ConversationData.SetValue<List<UserCart>>(StateKeys.UserCartInfo, UserInfo.ListuserCarts);

                if (context.ConversationData.TryGetValue<List<UserCart>>(StateKeys.UserCartInfo, out UserInfo.ListuserCarts))
                {
                    UserInfo.ListuserCarts.Add(userInfo.userCarts);
                }
                await DisplayCart(context,result);
                PromptDialog.Choice(context, this.End, new List<string> { "Checkout", "Continue", "Exit", "Cart" }, "Select Choice", "Not a valid options", 3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task End(IDialogContext context, IAwaitable<string> result)
        {
            string Selected = await result;
            if (Selected == "Checkout")
            {
                await context.PostAsync($"A Email will be sent to {userInfo.Address}");
                for (int index = 0; index < UserInfo.ListuserCarts.Count; index++)
                {
                    dal.Insertion(UserInfo.ListuserCarts[index].ProductName, UserInfo.ListuserCarts[index].Price, UserInfo.ListuserCarts[index].Description, userInfo.Name);
                }
                await StartAsync(context);
            }
            else if (Selected == "Exit")
            {
                await context.PostAsync("Thank you " + userInfo.Name + " for shopping with us");
                await StartAsync(context);
            }
            else if (Selected == "Continue")
            {
                this.ShowOptions(context);
            }
            else
            {
                PromptDialog.Choice(context, Cart, new List<string> { "Edit" }, "Your Cart", "Not a valid options", 3);

            }
        }

        private async Task Cart(IDialogContext context, IAwaitable<string> result)
        {
            string activity = await result;
            try
            {
                if (activity == "Edit")
                {
                    if (context.ConversationData.TryGetValue<List<UserCart>>(StateKeys.UserCartInfo, out UserInfo.ListuserCarts))
                    {
                        for (int index = 0; index < UserInfo.ListuserCarts.Count; index++)
                        {
                            ListStoringCart.Add(UserInfo.ListuserCarts[index].ProductName + " " + UserInfo.ListuserCarts[index].Description + " of Rs." + UserInfo.ListuserCarts[index].Price);
                        }
                    }
                    PromptDialog.Choice(context, UpdateCart, ListStoringCart, "Select to remove", "Not a valid options", 3);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task UpdateCart(IDialogContext context, IAwaitable<string> result)
        {
            List<string> str = new List<string>();
            string activity = await result;
            for (int index = 0; index < UserInfo.ListuserCarts.Count; index++)
            {
                if(ListStoringCart[index].ToString()==activity)
                {
                    UserInfo.ListuserCarts.RemoveAt(index);
                }
            }
            await DisplayCart(context,result);
            await End(context,result);
        }

        private async Task DisplayCart(IDialogContext context, IAwaitable<string> result)
        {
            for (int index = 0; index < UserInfo.ListuserCarts.Count; index++)
            {
                await context.PostAsync("You have selected " + UserInfo.ListuserCarts[index].ProductName + " " + UserInfo.ListuserCarts[index].Description + " of Rs." + UserInfo.ListuserCarts[index].Price);
            }
        }
    }
}