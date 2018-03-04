using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class VisitMuseMenu : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(OnMessageReceived);
            ShowMenu(context);
            return Task.CompletedTask;
        }

        private async Task OnMessageReceived(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as IMessageActivity;
            var actionMessage = activity.Text;
            switch (actionMessage)
            {
                case Constants.ConstantStrings.Info:

                    // Where?
                    var card = await Helper.DialogHelper.GenerateLocationCard();
                    await Helper.DialogHelper.sendAttachment(context, card.ToAttachment());

                    break;
                case Constants.ConstantStrings.VisitMuse:
                    context.Call<object>(new VisitMuse(), this.GoBack);
                    break;
                default:
                    context.Done<object>(null);
                    break;
            }
        }

        private  void ShowMenu(IDialogContext context)
        {
            string[] options = { Constants.ConstantStrings.Info, Constants.ConstantStrings.VisitMuse };
            var replyMessage = Helper.DialogHelper.createChoiceMessage(context, options, $@"Come posso aiutarti?");
            context.PostAsync(replyMessage);
        }

        private async Task GoBack(IDialogContext context, IAwaitable<object> result)
        {
            if (await result == null)
            {
                context.Done<object>(null);
            }
        }
    }
}