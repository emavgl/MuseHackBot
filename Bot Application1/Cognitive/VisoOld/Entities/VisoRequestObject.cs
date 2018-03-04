using System.Runtime.Serialization;

namespace ChatBot.Cognitive.Viso
{
    [DataContract]
    public class VisoRequestRootObject
    {
        [DataMember]
        public string url { get; set; }
    }
}