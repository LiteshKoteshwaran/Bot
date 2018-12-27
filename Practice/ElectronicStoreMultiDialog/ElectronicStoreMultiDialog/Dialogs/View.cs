using Chronic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStoreMultiDialog.Dialogs
{
    [Serializable]
    public class View : IDialog<object>
    {
        List<string> list = new List<string>();
        DAL dal = new DAL();
        static string Category, Brand;
        static string[] Description = new string[10];
        static string[] Price = new string[10];
        public async Task StartAsync(IDialogContext context)
        {
                await MessageReceivedAsync(context);

        }
        private async Task MessageReceivedAsync(IDialogContext context)
        {
            List<string> ProductsList = new List<string>();
            try
            {
                if (Luis.EntityType.Contains(Entities.AvailableProductName))
                {
                    ProductsList  = dal.ShowAvailable(Entities.Product);
                    for (int i = 0; i < Luis.Entity.Count; i++)
                    {
                        for (int j = 0; j < ProductsList.Count; j++)
                        {
                            if (Luis.Entity[i].ToLower() == ProductsList[j].ToLower())
                            {
                                Description[i] = dal.GetProductDetail(ProductsList[j], Entities.Description);
                                Price[i] = dal.GetProductDetail(ProductsList[j], Entities.Price);
                                await context.PostAsync("Description: " + Description[i] + " Price: " + Price[i]);
                            }
                        }
                    }
                    await context.PostAsync("Would you want me to add these products into cart");
                    context.Wait(ResumeAfterOptionSelected);
                }
                if (Luis.Entity.Count < 2)
                {
                    //if (Luis.EntityType.Contains(Entities.AvailableProductName))
                    //{
                    //    for (int i = 0; i < Luis.Entity.Count; i++)
                    //    {
                    //        await context.PostAsync(Description + Price);
                    //    }
                    //}

                    if (Luis.EntityType[0].ToLower() == Entities.Product.ToLower() || Luis.EntityType[0].ToLower() == Entities.Category.ToLower() || Luis.EntityType[0].ToLower() == Entities.Brand.ToLower() || Luis.EntityType[0].ToLower() == Entities.Cart.ToLower() && (!Luis.Entity.Contains(Entities.AvailableProductName)))
                    {
                        list = dal.ShowAvailable(Luis.EntityType[0]);
                    }

                    if (Luis.EntityType[0].ToLower() == Entities.AvailableCategories.ToLower())
                    {
                        list = dal.ShowBrandOnSelection(Luis.Entity[0]);
                    }

                    if (Luis.EntityType[0].ToLower() == Entities.AvailableBrands.ToLower())
                    {
                        for (int j = 0; j < StateKeys.ListOfBrands.Count; j++)
                        {
                            if (Luis.Entity[0].ToLower() == StateKeys.ListOfBrands[j].ToLower())
                            {
                                list = dal.ShowAvailableProductsOnSelection(StateKeys.ListOfBrands[j]);
                            }
                        }
                    }
                    //PromptDialog.Choice(context, ResumeAfterOptionSelected, list, Luis.Entity[0], "Not a valid options", 3);
                    foreach (var ele in list)
                    {
                        await context.PostAsync(ele);
                    }
                }
                if (Luis.Entity.Count >= 2&& !Luis.EntityType.Contains(Entities.AvailableProductName))
                {
                    if ((Luis.EntityType[0].ToLower() == Entities.AvailableBrands.ToLower() || Luis.EntityType[1].ToLower() == Entities.AvailableBrands.ToLower()) && (Luis.EntityType[0].ToLower() == Entities.AvailableCategories.ToLower() || Luis.EntityType[1].ToLower() == Entities.AvailableCategories.ToLower()) && (!(Luis.Entity.Contains(Entities.AvailableProductName)/*&&Luis.Entity.Contains(Entities.AvailableProductName)*/) ))
                    {
                        for (int i = 0; i < Luis.Entity.Count; i++)
                        {
                            for (int j = 0; j < StateKeys.ListOfCategory.Count; j++)
                            {
                                if (Luis.Entity[i].ToLower() == StateKeys.ListOfCategory[j].ToLower())
                                {
                                    Category = StateKeys.ListOfCategory[j];
                                }
                            }
                            for (int k = 0; k < StateKeys.ListOfBrands.Count; k++)
                            {
                                if (Luis.Entity[i].ToLower() == StateKeys.ListOfBrands[k].ToLower())
                                {
                                    Brand = StateKeys.ListOfBrands[k];
                                }
                            }
                        }
                        list = dal.ShowAvailableProductsOnSelectionOfCategoryAndBrand(Category, Brand);
                    }
                    else if ((Luis.EntityType[0].ToLower() == Entities.Brand.ToLower() | Luis.EntityType[1].ToLower() == Entities.Brand.ToLower()) && (Luis.EntityType.Contains(Entities.AvailableCategories)))
                    {
                        for (int i = 0; i < Luis.Entity.Count; i++)
                        {
                            for (int j = 0; j < StateKeys.ListOfCategory.Count; j++)
                            {
                                if (Luis.Entity[i] == StateKeys.ListOfCategory[j])
                                {
                                    list = dal.ShowBrandOnSelection(StateKeys.ListOfCategory[j]);
                                }
                            }
                        }
                    }
                    //if (Luis.EntityType[0].ToLower() == Entities.AvailableCategories.ToLower()| Luis.EntityType[1].ToLower() == Entities.AvailableCategories.ToLower())
                    //{
                    //    for (int j = 0; j < StateKeys.ListOfBrands.Count; j++)
                    //    {
                    //        if (Luis.Entity[0].ToLower() == StateKeys.ListOfBrands[j].ToLower())
                    //        {
                    //            list = dal.ShowAvailableProductsOnSelectionOfCategoryAndBrand(Luis.EntityType[1].ToLower(), StateKeys.ListOfBrands[j]);
                    //        }
                    //    }
                    //}

                    //PromptDialog.Choice(context, ResumeAfterOptionSelected, list, "Contents based on your selection", "Not a valid options", 3);
                    foreach(var ele in list)
                    {
                        await context.PostAsync(ele);
                    }

                }
                await context.PostAsync("What to next?????");
                context.Wait(ResumeAfterOptionSelected);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task ResumeAfterOptionSelected(IDialogContext context, IAwaitable<Object> result)
        {
            var message = await result as Activity;
            if(message.Text=="yes")
            {
                int i = (Luis.Entity.Count / 2);
                for(;i<Luis.Entity.Count;i++)
                {
                    dal.AddtoCart(Luis.Entity[i]);
                }
            }
            await Luis.IdentifyUserQueryUsingLuis(message.Text);
            await IntentOperations.IdentifyUserIntent(context, result);
        }
    }
}