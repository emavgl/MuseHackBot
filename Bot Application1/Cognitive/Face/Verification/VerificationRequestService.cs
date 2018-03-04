using Bot_Application1.Cognitive;
using ChatBot.Cognitive.Verification.Entities;
using ChatBot.Cognitive.Viso;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace ChatBot.Cognitive.Verification
{
    public class VerificationRequestService : CognitiveRequestService, IVerificationRequest
    {
        public VerificationRequestService(string subscriptionKey, string uriBase) : base(subscriptionKey, uriBase) { }

        /// <inheritdoc/>
        public async Task<VerificationResponseRootObject> MakeVerificationRequest(string faceId1, string faceId2)
        {
            return await GetVerificationResponse(faceId1, faceId2);
        }

        /// <summary>
        /// Gets the verification response from verification api of Azure.
        /// </summary>
        /// <param name="faceId1">id of the first face.</param>
        /// <param name="faceId2">id of the second face.</param>
        /// <returns>the task that contains the response.</returns>
        private async Task<VerificationResponseRootObject> GetVerificationResponse(string faceId1, string faceId2) {
            // Initialization of the http client.
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Initiating the request object.
            VerificationRequestRootObject requestObj = new VerificationRequestRootObject();
            requestObj.faceId1 = faceId1;
            requestObj.faceId2 = faceId2;

            // Serializing the request object.
            string requestJson = JsonConvert.SerializeObject(requestObj);

            // Adding the json to the header.
            HttpContent content = new StringContent(requestJson);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Executing the rest API call.
            HttpResponseMessage responseMessage = await client.PostAsync(uriBase, content);

            if (responseMessage.IsSuccessStatusCode)
            {
                // Get the JSON response.
                string contentString = await responseMessage.Content.ReadAsStringAsync();

                // Deserialize the JSON.
                VerificationResponseRootObject data = JsonConvert.DeserializeObject<VerificationResponseRootObject>(contentString);

                // Returning the value.
                return await Task.FromResult<VerificationResponseRootObject>(data);
            }
            else
            {
                // Throwing an exception.
                throw new UnsuccessfulStatusCodeException();
            }
        }

    }
}