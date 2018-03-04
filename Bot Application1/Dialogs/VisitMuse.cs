using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Bot_Application1.Models;
using Bot_Application1.Services;
using ChatBot.Cognitive.Face;
using ChatBot.Cognitive.Viso;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class VisitMuse : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Mandami un tuo selfie 🤳, scommettiamo che indovino la tua eta'?");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as IMessageActivity;
            if (activity.Attachments != null && activity.Attachments.Any())
            {
                var attachment = activity.Attachments.First();
                if (!attachment.ContentType.Contains("image"))
                {
                    await context.PostAsync("Ho ricevuto un file, ma non mi sembra una tua foto...");
                }
                else
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        var responseMessage = await httpClient.GetAsync(attachment.ContentUrl);
                        byte[] img = await responseMessage.Content.ReadAsByteArrayAsync();
                        string subscriptionKey = WebConfigurationManager.AppSettings["CognitiveSubscriptionKey"];
                        string uriBase = WebConfigurationManager.AppSettings["CognitiveBaseURI"];
                        VisoRequestService viso = new VisoRequestService(subscriptionKey, uriBase);
                        var visoResponse = await viso.MakeVisoRequest(img);
                        var age = visoResponse.faceAttributes.age;
                        var gender = visoResponse.faceAttributes.gender;
                        var photoUrl = attachment.ContentUrl;
                        await context.PostAsync($@"Hai {Math.Round(Convert.ToDecimal(age.Split('.')[0]), 0)} anni ... ne dimostri molto meno!");
                        await context.PostAsync("Foto stupenda! 🤗 Ora siamo pronti per iniziare la visita. Ti suggerirò alcuni percorsi per te");
                        saveUserProfile(context, age, gender, photoUrl);

                        await context.PostAsync("I percorsi più adatti a te possono essere ...");
                        var reply3 = context.MakeMessage();
                        Attachment[] cards = {
                                        Helper.DialogHelper.createVisitCard("Un'avventura tra i ghiacci ti aspetta al quarto piano", "http://www.muse.it/it/Esplora/percorso-espositivo/Piano-Quarto/Pagine/Piano-Quarto.aspx", HttpContext.Current.Server.MapPath("~/Assets/ghiacci.jpg")),
                                        Helper.DialogHelper.createVisitCard("Una vista spettacolare sulla valle dell'adige",  "http://www.muse.it/it/Esplora/percorso-espositivo/Terrazza/Pagine/Terrazza.aspx", HttpContext.Current.Server.MapPath("~/Assets/terrazza.jpg")),
                                    };
                        reply3.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                        reply3.Attachments = cards;
                        await context.PostAsync(reply3);

                        await context.PostAsync($@"Però...somigli a qualcuno...");
                    
                        string faceApiKey = WebConfigurationManager.AppSettings["FACE_API_KEY"];
                        string verificationBaseURL = WebConfigurationManager.AppSettings["VERIFICATION_API_BASE_URI"];

                        FacesVerificationRequestService ser = new FacesVerificationRequestService(faceApiKey, uriBase, verificationBaseURL);
                        var verification = await ser.GetImagesConfidence(img);

                        string output = await PhotoMaker.mergeImages(photoUrl, verification + "%");
                        var reply1 = context.MakeMessage();

                        var imageData = Convert.ToBase64String(File.ReadAllBytes(output));

                        var attch = new Attachment
                        {
                            Name = "Somiglianza con l'uomo di neanderthal: " + verification + "%",
                            ContentType = "image/jpeg",
                            ContentUrl = $"data:image/jpeg;base64,{imageData}"
                        };

                        reply1.Attachments = new List<Attachment> { attch };

                        await context.PostAsync(reply1);

                        await context.PostAsync("Condividi la foto e presentati alla cassa per avere un gadget in regalo!");

                        context.Call<object>(new FeedbackDialog(), this.GoBack);
                    }
                }
            }
            else
            {
                await context.PostAsync("Apri la camera e sorridi! 😄");
            }
            //context.Wait(MessageReceivedAsync);
        }

        private void saveUserProfile(IDialogContext context, string age, string gender, string photoUrl)
        {
            UserProfile _profile;
            if (context.UserData.TryGetValue(@"profile", out _profile))
            {
                // UserProfile exists already
                // You can now set age and gender
                char a = '.';
                age = age.Split('.')[0];
                _profile.Age = (int)Math.Round(float.Parse(age));
                _profile.Gender = (gender.ToLower().Equals("male")) ? UserProfile.GenderEnum.Male : UserProfile.GenderEnum.Female;
                _profile.PhotoUrl = photoUrl;
            }

            // Save profile in UserData storage
            context.UserData.SetValue(@"profile", _profile);
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