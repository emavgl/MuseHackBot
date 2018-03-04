using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application1.Cognitive.Emotion
{
    public interface IEmotionRequest
    {
        /// <summary>
        /// Makes a request to the APIs for phrase emotion detection.
        /// </summary>
        /// <param name="phrase">phrase to detect the emotion of.</param>
        /// <returns>the score of the emotion.</returns>
        int MakeEmotionRequest(string phrase);

    }
}