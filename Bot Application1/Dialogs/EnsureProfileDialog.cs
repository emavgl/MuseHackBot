using Bot_Application1.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class EnsureProfileDialog : IDialog<UserProfile>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(OnMessageReceived);
            await EnsureProfileName(context);
        }

        private async Task OnMessageReceived(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as IMessageActivity;
            string actionMessage = activity.Text;
            switch (actionMessage)
            {
                case Constants.ConstantStrings.YesGoToMuse:
                    context.Call<object>(new VisitMuseMenu(), this.GoBack);
                    break;
                case Constants.ConstantStrings.AlreadyWentThere:
                    await context.PostAsync("Da qui si avrà accesso all'area feedback e suggerimenti");
                    break;
                default:
                    // The message contains the name of the user
                    await AskChoice(context, actionMessage);
                    break;
            }
        }

        UserProfile _profile;
        private async Task EnsureProfileName(IDialogContext context)
        {
            if (!context.UserData.TryGetValue(@"profile", out _profile))
            {
                // Does not exists any User's profile yet
                _profile = new UserProfile();
            }

            if (string.IsNullOrWhiteSpace(_profile.Name))
            {
                // If there is no name set

                // Check if there is a name
                dynamic json = context.Activity.ChannelData;
                try
                {
                    string nickname = context.Activity.From.Name;
                    string name = (!String.IsNullOrWhiteSpace(json.message.from.first_name)) ? json.message.from.first_name : nickname; 
                    if (!String.IsNullOrWhiteSpace(name))
                    {
                        await AskChoice(context, name);
                    }
                    else
                    {
                        await context.PostAsync(@"Ciao, io mi chiamo Musa. Tu?");
                    }
                }
                catch {
                    await context.PostAsync(@"Ciao, io mi chiamo Musa. Tu?");
                }
            }
            else
            {
                // _profile.Name is already defined
                await AskChoice(context, _profile.Name);
            }
        }


        private async Task AskChoice(IDialogContext context, string name)
        {
            _profile.Name = name;
            context.UserData.SetValue("profile", _profile);
            string[] options = { Constants.ConstantStrings.YesGoToMuse, Constants.ConstantStrings.AlreadyWentThere };
            CustomMessages cm = WebApiApplication.BotStrings["welcome.message"];
            cm.text = String.Format(cm.text, _profile.Name);
            await context.PostAsync(cm.getActivityFromMessage(context));
        }

        private async Task GoBack(IDialogContext context, IAwaitable<object> result)
        {
            var param = await result;
            if (param != null)
            {
                context.Done(_profile);
            }
            else
            {
                await AskChoice(context, _profile.Name);
            }
        }
    }
}