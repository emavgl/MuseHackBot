using Bot_Application1.Cognitive;
using Bot_Application1.Cognitive.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChatBot.Cognitive.Viso
{
    /// <summary>
    /// Helper class responsible about making the API calls for the Viso cognitive service.
    /// </summary>
    public class VisoRequestService : CognitiveRequestService, IVisoRequest
    {
   
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="subscriptionKey">key given by Microsoft Azure for accessing the Viso service.</param>
        /// <param name="uriBase">endpoint uri that corresponds to the url of the Microsoft Azure instance.</param>
        public VisoRequestService(string subscriptionKey, string uriBase) : base(subscriptionKey, uriBase) { }

        /// <inheritdoc/>
        public async Task<VisoResponseRootObject> MakeVisoRequest(string imagePath, bool isUrl)
        {
            if (isUrl) return await GetVisoResponse(imagePath);
            else return await GetVisoResponse(CognitiveUtils.GetImageFromPath(imagePath));
        }

        /// <inheritdoc/>
        public async Task<VisoResponseRootObject> MakeVisoRequest(byte[] image)
        {
            return await GetVisoResponse(image);
        }

        /// <summary>
        /// Makes a request to the server and parses the result using the image byte array.
        /// </summary>
        /// <param name="image">byte array representing the image.</param>
        /// <returns>task that contains the response.</returns>
        private async Task<VisoResponseRootObject> GetVisoResponse(byte[] image) {
            // Initialization of the http client.
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Parameters that we want from the API.
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender";

            // Assemble the URI for the REST API call.
            string uri = uriBase + requestParameters;

            // Adding the byte array to the header.
            using (ByteArrayContent content = new ByteArrayContent(image))
            {
                // Adding the header that will contain our byte array.
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Executing the REST API call.
                HttpResponseMessage responseMessage = await client.PostAsync(uri, content);

                if (responseMessage.IsSuccessStatusCode)
                {
                    // Get the JSON response.
                    string contentString = await responseMessage.Content.ReadAsStringAsync();

                    // Deserialize the JSON.
                    List<VisoResponseRootObject> data = JsonConvert.DeserializeObject<List<VisoResponseRootObject>>(contentString);

                    // Returning the value.
                    return await Task.FromResult<VisoResponseRootObject>(data[0]);
                }
                else
                {
                    // Throwing an exception.
                    throw new UnsuccessfulStatusCodeException();
                }
            }
        }

        /// <summary>
        /// Makes a request to the server and parses the result using the image url.
        /// </summary>
        /// <param name="imageUrl">url of the image.</param>
        /// <returns>task that contains the response.</returns>
        private async Task<VisoResponseRootObject> GetVisoResponse(string imageUrl) {
            // Initialization of the http client.
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Parameters that we want from the API.
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender";

            // Assemble the URI for the REST API call.
            string uri = uriBase + requestParameters;

            // Initiating the request object.
            VisoRequestRootObject requestObj = new VisoRequestRootObject();
            requestObj.url = imageUrl;

            // Serializing the request object.
            string requestJson = JsonConvert.SerializeObject(requestObj);

            // Adding the json to the header.
            HttpContent content = new StringContent(requestJson);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Executing the rest API call.
            HttpResponseMessage responseMessage = await client.PostAsync(uri, content);

            if (responseMessage.IsSuccessStatusCode)
            {
                // Get the JSON response.
                string contentString = await responseMessage.Content.ReadAsStringAsync();

                // Deserialize the JSON.
                List<VisoResponseRootObject> data = JsonConvert.DeserializeObject<List<VisoResponseRootObject>>(contentString);

                // Returning the value.
                return await Task.FromResult<VisoResponseRootObject>(data[0]);
            }
            else
            {
                // Throwing an exception.
                throw new UnsuccessfulStatusCodeException();
            }
        }
    }
}