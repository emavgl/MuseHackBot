using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.Cognitive
{
    public class CognitiveRequestService
    {

        /// <summary>
        /// Key of the Microsoft Azure cognitive service.
        /// </summary>
        protected string subscriptionKey;
        /// <summary>
        /// Base uri that represents the endpoint of Azure.
        /// </summary>
        protected string uriBase;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="subscriptionKey">key given by Microsoft Azure for accessing the cognitive services.</param>
        /// <param name="uriBase">endpoint uri that corresponds to the url of the Microsoft Azure instance.</param>
        public CognitiveRequestService(string subscriptionKey, string uriBase) {
            this.subscriptionKey = subscriptionKey;
            this.uriBase = uriBase;
        }

    }
}