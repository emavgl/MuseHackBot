using Bot_Application1.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Location;
using Microsoft.Bot.Builder.Location.Bing;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Bot_Application1.Helper
{
    public static class DialogHelper
    {
        public static async Task SendTypingAndWait(IDialogContext context, int timeout = 5000)
        {
            var reply = context.MakeMessage();
            reply.Type = Microsoft.Bot.Connector.ActivityTypes.Typing;
            reply.Text = String.Empty;
            await context.PostAsync(reply);
            await Task.Delay(5000);
        }

        public static async Task<HeroCard> GenerateLocationCard()
        {
            var address = "Corso del Lavoro e della Scienza 3 - 38122 Trento";
            var resourceManager = new LocationResourceManager();
            var mapsAPI = WebConfigurationManager.AppSettings["BingMapsMasterKey"];
            var carBuilder = new LocationCardBuilder(mapsAPI, resourceManager);
            var bingService = new BingGeoSpatialService(mapsAPI);
            var locationSet = await bingService.GetLocationsByQueryAsync(address);
            var addressUrl = "https://www.google.it/maps/place/" + address;
            var card = carBuilder.CreateHeroCards(locationSet.Locations).ToList().First();
            card.Title = "Info";

            string text = String.Empty;
            text += $@"Telefono: +39 0461270311" + "\n\n";
            text += $@"Email: museinfo@muse.it" + "\n\n";
            text += $@"Indirizzo: {address}";

            card.Text = text;
            card.Buttons = new[] { new CardAction(ActionTypes.OpenUrl, @"Mappa", value: addressUrl),
                                   new CardAction(ActionTypes.OpenUrl, @"Contatti", value: "http://www.muse.it/it/contatti/Pagine/default.aspx"),
                                   new CardAction(ActionTypes.OpenUrl, @"Orari e Tariffe", value: "http://www.muse.it/it/visita/orari-tariffe/Pagine/Home.aspx")};
            return card;
        }

        public static async Task sendAttachment(IDialogContext context, Attachment attachment)
        {
            var message = context.MakeMessage();
            message.Attachments.Add(attachment);
            await context.PostAsync(message);
        }

        public static IMessageActivity createChoiceMessage(IDialogContext context, IEnumerable<string> options, string message)
        {
            var newMessage = context.MakeMessage();

            List<CardAction> cardAction = new List<CardAction>();
            foreach (var item in options)
            {
                CardAction c = new CardAction(ActionTypes.PostBack, item, value: item);
                cardAction.Add(c);
            }

            var heroCard = new HeroCard
            {
                Text = message,
                Buttons = cardAction
            };

            var attach1 = heroCard.ToAttachment();
            newMessage.Attachments.Add(attach1);
            return newMessage;
        }

        public static Attachment createVisitCard(string description, string url, string imageUrl)
        {
            var images = new List<CardImage> { new CardImage(imageUrl) };

            List<CardAction> cardAction = new List<CardAction>();
            CardAction c = new CardAction(ActionTypes.OpenUrl, @"Apri", value: url);
            cardAction.Add(c);

            var hc =  new HeroCard
            {
                Text = description,
                Buttons = cardAction
            };

            var atc = hc.ToAttachment();
            return atc;
        }

    }
}