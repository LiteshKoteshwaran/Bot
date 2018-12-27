using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStoreMultiDialog
{
    [Serializable]
    public class Cards 
    {
        //public static async Task HeroCard(IDialogContext context, string NameList, string Description, string Img, string Purpose)
        //{
        //    var resultMessage = context.MakeMessage();
        //    resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        //    resultMessage.Attachments = new List<Attachment>();

        //    foreach (var Item in List)
        //    {
        //        HeroCard heroCard = new HeroCard()
        //        {
        //            Title = Name,
        //            Subtitle = Description,
        //            Images = new List<CardImage>()
        //            {
        //                new CardImage(){Url = Img}
        //            },
        //            Buttons = new List<CardAction>()
        //            {
        //                     new CardAction(ActionTypes.ImBack, Purpose, value: Name),
        //            }
        //        };
        //        resultMessage.Attachments.Add(heroCard.ToAttachment());
        //    }
        //    await context.PostAsync(resultMessage);
        //}
    }
}