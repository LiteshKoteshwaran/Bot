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
        ConnetionMannger connetionMannger;
        public static string BrandSelected, CategorySelected, SubCategorySelected, TypeSelected;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            //return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            try
            {
                if (activity.Text == "hey" || activity.Text == "hi" || activity.Text == "hai" && !(context.UserData.TryGetValue("Name", out UserInfo.Name)))
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
                else if (!context.UserData.TryGetValue("Name", out UserInfo.Name))
                {
                    try
                    {
                        UserInfo.Name = activity.Text;
                        context.UserData.SetValue("Name", UserInfo.Name);
                        await context.PostAsync($"Hey {UserInfo.Name} Enter your Email to continue");
                        ResumeAfterOptionDialog(context, result);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (!context.UserData.TryGetValue("Email", out UserInfo.Email))
                {
                    UserInfo.Email = activity.Text;
                    context.UserData.SetValue("Email", UserInfo.Email);
                    await context.PostAsync("Your Address????");
                    ResumeAfterOptionDialog(context, result);
                }
                else if (!context.UserData.TryGetValue(UserInfo.Address, out UserInfo.Address))
                {
                    UserInfo.Address = activity.Text;
                    context.UserData.SetValue("Address", UserInfo.Address);
                    this.ShowOptions(context);
                }
                else if (context.UserData.TryGetValue("Name", out UserInfo.Name))
                {
                    await context.PostAsync($"welcome back {UserInfo.Name}");
                    this.ShowOptions(context);
                }
            }
            catch(Exception ex)
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
                //string Query = "select distinct b.Name from  Brand b join Product p on b.Id = p.BrandId join Category c  on C.Id=p.CategoryId where c.Name ='" + BrandSelected + "'";
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


        private async Task DisplaySelection(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                UserInfo userInfo = new UserInfo();
                DAL dal = new DAL();
                connetionMannger = new ConnetionMannger();
                TypeSelected = await result;
                string Query = "select Price from Product where Title='" + TypeSelected + "'";
                long Price = Convert.ToInt64(dal.GetPrice(Query));
                UserCart.TotalPrice += Price;

                connetionMannger.InsertIntoCart(TypeSelected, UserCart.TotalPrice);
                await context.PostAsync("You have selected " + BrandSelected + " " + CategorySelected + " of Rs." + UserCart.TotalPrice);
                PromptDialog.Choice(context, this.End, new List<string> { "Checkout", "Continue", "Exit", "Cart" }, "Select Choice", "Not a valid options", 3);
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
            catch(Exception ex)
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
                //string Query = "select p.Description from product p join brand b on B.Id = p.brandId join category c on p.CategoryId = c.Id where b.Name = '" + BrandSelected + "'" + "and c.Name = '" + CategorySelected + "'";
                List<string> Brand = dal.GetCategory(Query);
                PromptDialog.Choice(context, DisplaySelection, Brand, "Please choose one from the options", "Invalid Option type. Please try again", 3);
            }
            catch
            {
                await context.PostAsync("Thanks");
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task End(IDialogContext context, IAwaitable<string> result)
        {
            string Selected = await result;
            if (Selected == "Checkout")
            {
                await context.PostAsync($"A Email will be sent to {UserInfo.Address}");
            }
            else if (Selected == "Exit")
            {
                await context.PostAsync("Thank you " + UserInfo.Name + " for shopping with us");
            }
            else if (Selected == "Continue")
            {
                this.ShowOptions(context);
            }
            else
            {

            }
        }
    }
}