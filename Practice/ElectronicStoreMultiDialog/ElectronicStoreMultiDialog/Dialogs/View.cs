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
        static string Description, Price, Category, Brand;

        public async Task StartAsync(IDialogContext context)
        {
                await MessageReceivedAsync(context);

        }
        private async Task MessageReceivedAsync(IDialogContext context)
        {
            try
            {
                if (Luis.EntityType.Contains(Entities.AvailableProductName))
                {
                    list = dal.ShowAvailable(Entities.Product);
                    for (int i = 0; i <list.Count; i++)
                    {
                        if (Luis.Entity[i].ToLower() == list[i].ToLower())
                        {
                            Description = dal.GetProductDetail(list[i], Entities.Description);
                            Price = dal.GetProductDetail(list[i], Entities.Price);
                        }
                        await context.PostAsync(Description+Price);
                    }
                }

                if (Luis.Entity.Count < 2)
                {
                    if (Luis.EntityType[0].ToLower() == Entities.Product.ToLower() || Luis.EntityType[0].ToLower() == Entities.Category.ToLower() || Luis.EntityType[0].ToLower() == Entities.Brand.ToLower() || Luis.EntityType[0].ToLower() == Entities.Cart.ToLower()/*|| Luis.EntityType[0].ToLower() == Entities.AvailableCategories.ToLower()*/)
                    {
                        list = dal.ShowAvailable(Luis.EntityType[0]);
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
                    if (Luis.EntityType[0].ToLower() == Entities.AvailableProductPrice.ToLower())
                    {
                        for (int j = 0; j < StateKeys.ListOfBrands.Count; j++)
                        {
                            if (Luis.Entity[0].ToLower() == StateKeys.ListOfBrands[j].ToLower())
                            {
                                list = dal.ShowAvailableProductsOnSelection(StateKeys.ListOfBrands[j]);
                            }
                        }
                    }
                    if (Luis.EntityType[0].ToLower() == Entities.AvailableProductDescription.ToLower())
                    {
                        for (int j = 0; j < StateKeys.ListOfBrands.Count; j++)
                        {
                            if (Luis.Entity[0].ToLower() == StateKeys.ListOfBrands[j].ToLower())
                            {
                                list = dal.ShowAvailableProductsOnSelection(StateKeys.ListOfBrands[j]);
                            }
                        }
                    }
                    if (Luis.EntityType[0].ToLower() == Entities.Category.ToLower())
                    {
                        for (int j = 0; j < StateKeys.ListOfBrands.Count; j++)
                        {
                            if (Luis.Entity[0].ToLower() == StateKeys.ListOfBrands[j].ToLower())
                            {
                                list = dal.ShowAvailableProductsOnSelection(StateKeys.ListOfBrands[j]);
                            }
                        }
                    }
                    PromptDialog.Choice(context, ResumeAfterOptionSelected, list, Luis.Entity[0], "Not a valid options", 3);
                }
                if (Luis.Entity.Count >= 2)
                {
                    if ((Luis.EntityType[0].ToLower() == Entities.AvailableBrands.ToLower() || Luis.EntityType[1].ToLower() == Entities.AvailableBrands.ToLower()) && (Luis.EntityType[0].ToLower() == Entities.Category.ToLower() || Luis.EntityType[1].ToLower() == Entities.Category.ToLower()))
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
                    }
                    else if ((Luis.EntityType[0].ToLower() == Entities.Brand.ToLower() | Luis.EntityType[1].ToLower() == Entities.Brand.ToLower()) && (Luis.EntityType.Contains(Entities.AvailableCategories)))
                    {
                        for (int i = 0; i < Luis.Entity.Count; i++)
                        {
                            for (int j = 0; j < StateKeys.ListOfCategory.Count; j++)
                            {
                                if (Luis.Entity[i]==StateKeys.ListOfCategory[j])
                                {
                                    list = dal.ShowBrandOnSelection(StateKeys.ListOfCategory[j]);
                                }
                            }
                        }
                    }
                    list = dal.ShowAvailableProductsOnSelectionOfCategoryAndBrand(Category, Brand);
                    PromptDialog.Choice(context, ResumeAfterOptionSelected, list, "Contents based on your selection", "Not a valid options", 3);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task ResumeAfterOptionSelected(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as Activity;
            Luis.IdentifyUserQueryUsingLuis(message.Text);
            await IntentOperations.IdentifyUserIntent(context, result);
        }
        public static async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            View view = new View();
            await view.ResumeAfterOptionSelected(context,result);
        }
    }
}