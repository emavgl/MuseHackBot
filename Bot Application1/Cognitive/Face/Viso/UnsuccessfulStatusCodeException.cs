using System;

namespace ChatBot.Cognitive.Viso
{
    /// <summary>
    /// Exception thrown when the Viso APIs return an unsuccessfull status code.
    /// </summary>
    public class UnsuccessfulStatusCodeException : Exception
    {
        public override string Message => "The service returned an unsuccessfull status code.";
    }
}