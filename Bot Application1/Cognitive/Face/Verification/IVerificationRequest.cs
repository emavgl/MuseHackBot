using ChatBot.Cognitive.Verification.Entities;
using ChatBot.Cognitive.Viso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChatBot.Cognitive.Verification
{
    public interface IVerificationRequest
    {

        /// <summary>
        /// Makes a request to the verification API from Microsoft.
        /// </summary>
        /// <param name="faceId1">first face id.</param>
        /// <param name="faceId2">second face id.</param>
        /// <returns>the task that contains the response.</returns>
        Task<VerificationResponseRootObject> MakeVerificationRequest(string faceId1, string faceId2);

    }
}