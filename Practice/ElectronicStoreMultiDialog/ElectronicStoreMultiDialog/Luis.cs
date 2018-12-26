using ElectronicStoreMultiDialog.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ElectronicStoreMultiDialog
{
    [Serializable]
    public class Luis
    {
        public static string intent;
        public static List<string> Entity = new List<string>();
        public static List<string> EntityType = new List<string>();
        public static string EntityForQuery;



        public static async Task IdentifyUserQueryUsingLuis(string text)
        {
            Entity.Clear();
            EntityType.Clear();
            EntityForQuery = "";
            LuisResponse Data = new LuisResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var responseInString = await client.GetStringAsync(@"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/424aeb9b-a563-4f48-b3cb-8840f6cec54b?staging=true&verbose=true&timezoneOffset=-360&subscription-key=9a1593730efc486cb5f8e6ca341b5a62&q="
                   + System.Uri.EscapeDataString(text));

                    Data = JsonConvert.DeserializeObject<LuisResponse>(responseInString);
                    intent = Data.topScoringIntent.intent;
                    for (int i = 0; i < Data.entities.Length; i++)
                    {
                        Entity.Add(Data.entities[i].entity);
                        EntityType.Add(Data.entities[i].type);
                        //    if (EntityType[i].ToLower() == Entities.AvailableBrands || EntityType[i].ToLower() == Entities.Category)
                        //    {
                        //        for (int j = 0; j < StateKeys.ListOfBrands.Count; j++)
                        //        {
                        //            if (Data.entities[i].entity.ToLower() == StateKeys.ListOfBrands[j].ToLower())
                        //            {
                        //                EntityForQuery = StateKeys.ListOfBrands[j];
                        //            }
                        //            if (Data.entities[i].entity.ToLower() == StateKeys.ListOfCategory[j].ToLower())
                        //            {
                        //                EntityForQuery = StateKeys.ListOfCategory[j];
                        //            }
                        //        }
                        //    }
                        //    if (Data.entities[i].entity.ToLower() == Entities.Cart.ToLower())
                        //    {
                        //        EntityForQuery = Entities.Cart;
                        //    }
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.ErrorLogging("", ex.Message, "", DateTime.Now);
                throw ex;
            }
        }

    }
    public class IntentOperations
    {
        public static async Task IdentifyUserIntent(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                //var message = await result as Activity;
                switch (Luis.intent)
                {
                    case Intents.View:
                        context.Call(new View(), ResumeAfterOptionDialog);
                        break;
                    case Intents.AddToCart:
                        context.Call(new Cart(), ResumeAfterOptionDialog);
                        break;
                    case Intents.RemoveFromCart:
                        context.Call(new Cart(), ResumeAfterOptionDialog);
                        break;
                    case Intents.Checkout:
                        RootDialog.Checkout(context, result);
                        break;
                    default:
                        await context.PostAsync("Can you be more meaningful");
                        break;
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private static Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            throw new NotImplementedException();
        }
    }
}