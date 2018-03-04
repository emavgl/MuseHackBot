using System;
using System.Threading.Tasks;
using System.Web;
using Bot_Application1.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class VisitByAge : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(OnMessagReceived);
            await StartVisit(context);
        }

        private async Task OnMessagReceived(IDialogContext context, IAwaitable<object> result)
        {
            var msg = await result as IMessageActivity;
            await context.PostAsync(msg.Text);
        }

        private async Task StartVisit(IDialogContext context)
        {
            await context.PostAsync("Foto stupenda! 🤗 Ora siamo pronti per iniziare la visita. Ti suggerirò alcuni percorsi per te");

            var reply = context.MakeMessage();
            Attachment[] cards = {
                                        Helper.DialogHelper.createVisitCard("Un'avventura tra i ghiacci ti aspetta al quarto piano", "http://www.muse.it/it/Esplora/percorso-espositivo/Piano-Quarto/Pagine/Piano-Quarto.aspx", HttpContext.Current.Server.MapPath("~/Assets/ghiacci.jpg")),
                                        Helper.DialogHelper.createVisitCard("Una vista spettacolare sulla valle dell'adige",  "http://www.muse.it/it/Esplora/percorso-espositivo/Terrazza/Pagine/Terrazza.aspx", HttpContext.Current.Server.MapPath("~/Assets/terrazza.jpg")),
                                    };
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = cards;
            await context.PostAsync(reply);
        }
    }
}