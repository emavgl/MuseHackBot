using Bot_Application1.Cognitive;
using ChatBot.Cognitive.Verification;
using ChatBot.Cognitive.Viso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChatBot.Cognitive.Face
{
    public class FacesVerificationRequestService
    {
        /// <summary>
        /// Url of the nehandertal image used to make a confront between you and him.
        /// </summary>
        private const string NEHANDERTAL_IMAGE_URL = @"http://www.ansa.it/webimages/foto_large/2012/3/21/1332336812882_NEANDERTHAL.jpg";

        private string subscriptionKey;
        private string visoUriBase;
        private string verificationUriBase;
        private VisoRequestService visoService;
        private VerificationRequestService verificationService;

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="subscriptionKey">face api subscription key.</param>
        /// <param name="visoUriBase">viso api base url.</param>
        /// <param name="verificationUriBase">verification api base url.</param>
        public FacesVerificationRequestService(string subscriptionKey, string visoUriBase, string verificationUriBase)
        {
            this.subscriptionKey = subscriptionKey;
            this.visoUriBase = visoUriBase;
            this.verificationUriBase = verificationUriBase;
            visoService = new VisoRequestService(this.subscriptionKey, this.visoUriBase);
            verificationService = new VerificationRequestService(this.subscriptionKey, this.verificationUriBase);
        }

        /// <summary>
        /// Calculates the confidence between two images.
        /// </summary>
        /// <param name="imagePath">path of the image.</param>
        /// <param name="isUrl">true if the image path is an url false if is a directory path.</param>
        /// <returns>a Task that contains the % of confidence.</returns>
        public async Task<Int32> GetImagesConfidence(string imagePath, bool isUrl) {
            var result1 = await visoService.MakeVisoRequest(imagePath, isUrl);
            var result2 = await visoService.MakeVisoRequest(NEHANDERTAL_IMAGE_URL, true);
            var verificationResult = await verificationService.MakeVerificationRequest(result1.faceId, result2.faceId);
            return await Task.FromResult<Int32>((Int32) (verificationResult.confidence * 100));
        }

        /// <summary>
        /// Calculates the confidence between two images.
        /// </summary>
        /// <param name="image">byte array representing the image.</param>
        /// <returns>a Task that contains the % of confidence.</returns>
        public async Task<Int32> GetImagesConfidence(byte[] image) {
            var result1 = await visoService.MakeVisoRequest(image);
            var result2 = await visoService.MakeVisoRequest(NEHANDERTAL_IMAGE_URL, true);
            var verificationResult = await verificationService.MakeVerificationRequest(result1.faceId, result2.faceId);
            return await Task.FromResult<Int32>((Int32)(verificationResult.confidence * 100));
        }
    }
}