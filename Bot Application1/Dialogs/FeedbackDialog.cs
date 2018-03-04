using Bot_Application1.Cognitive.Emotion;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class FeedbackDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Come è stata la tua esperienza al muse?");
            context.Wait(FeedbackMessage);
        }

        private async Task FeedbackMessage(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            EmotionRequestService emotion = new EmotionRequestService(ConfigurationManager.AppSettings["textAnalysis"].ToString(), "it");
            try
            {
                int emoRate = emotion.MakeEmotionRequest(message.Text);
                if (emoRate < 51)
                {
                    //commento negativo
                    await context.PostAsync(WebApiApplication.BotStrings["negativefeedback.message"].getActivityFromMessage(context));
                }
                else
                {
                    //commento positivo
                    await context.PostAsync(WebApiApplication.BotStrings["positivefeedback.message"].getActivityFromMessage(context));
                }

            }
            catch (Exception e)
            {
                int a = 5;
                throw;
            }

            await context.PostAsync(WebApiApplication.BotStrings["dintorni.message"].getActivityFromMessage(context));
            context.Done<object>(null);
        }

    }
}