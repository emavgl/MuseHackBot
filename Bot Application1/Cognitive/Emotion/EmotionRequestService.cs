using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Bot_Application1.Cognitive.Utils;
using ChatBot.Cognitive.Viso;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;

namespace Bot_Application1.Cognitive.Emotion
{
    public class EmotionRequestService : CognitiveRequestService, IEmotionRequest
    {

        private const string DOCUMENT_ID = "0";

        /// <summary>
        /// Language of the phrase.
        /// </summary>
        private string language;

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="subscriptionKey"></param>
        /// <param name="uriBase"></param>
        /// <param name="language"></param>
        public EmotionRequestService(string subscriptionKey, string language) : base(subscriptionKey, null) {
            this.language = language;
        }

        /// <inheritdoc/>
        public int MakeEmotionRequest(string phrase)
        {
            return (int)GetEmotionResponse(phrase);
        }

        /// <summary>
        /// Gets the emotion response.
        /// </summary>
        /// <param name="phrase">phrase to calculate the sentiment of.</param>
        /// <returns>the score of the emotion response</returns>
        private double GetEmotionResponse(string phrase) {
            // Initiating the text analytics client.
            ITextAnalyticsAPI client = new TextAnalyticsAPI();

            // Setting the client region.
            client.AzureRegion = AzureRegions.Westeurope;

            // Setting the client subscription key.
            client.SubscriptionKey = subscriptionKey;

            // Sending the request to the server and getting the results.
            SentimentBatchResult result = client.Sentiment(
                    new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput(language, DOCUMENT_ID, phrase)
                        }));

            // Returning the result
            return result.Documents[0].Score.Value * 100;
        }

    }
}