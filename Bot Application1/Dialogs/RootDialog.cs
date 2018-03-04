using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot_Application1.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(FirstMessageReceivedAsync);
            return Task.CompletedTask;
        }

        private Task FirstMessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Call<UserProfile>(new EnsureProfileDialog(), null);
            return Task.CompletedTask;
        }
    }
}